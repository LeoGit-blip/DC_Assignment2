const API_BASE_URL = '/api/user';
const API_Logout_URL = '/api/logout';
const API_Profile_URL = '/api/profile';
const API_Account_URL = '/api/account';
const API_Transaction_URL = '/api/transactionuser';
const API_Transfer_URL = '/api/transfer';
const API_Editprofile_URL = '/api/editprofile';

var data;

/* Index.cstml */
function loadView(status)
{
    let apiUrl = `${API_BASE_URL}`;

    switch (status) {
        case 'login':
            apiUrl = `${API_BASE_URL}/login`;
            break;
        case 'authview':
            apiUrl = `${API_BASE_URL}/authview`;
            break;
        case 'profile':
            apiUrl = `${API_Profile_URL}`;
            break;
        case 'editprofile':
            apiUrl = `${API_Editprofile_URL}`;
            break;
        case 'account':
            apiUrl = `${API_Account_URL}`;
            break;
        case 'transaction':
            apiUrl = `${API_Transaction_URL}`;
            break;
        case 'transfer':
            apiUrl = `${API_Transfer_URL}`;
            break;
        case 'logout':
            apiUrl = `${API_Logout_URL}`;
            disableUserHomeButtons();
            break;
        case 'error':
            apiUrl = `${API_BASE_URL}/error` ;
            break;
    }

    console.log("Loading view: " + apiUrl);

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(data => {
            console.log("Inside fetch load view");
            document.getElementById('main').innerHTML = data;
            if (status == 'profile') {
                performProfile();
            }
            if (status == 'account') {
                fetchAccountSummary();
            }
            if (status == 'transaction') {
                loadTransactionHistory();
            } 
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function fetchAccountSummary() {
    const username = encodeURIComponent(data.UserName);
    fetch(`/api/account/summary/${username}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(acc => {
            console.log(acc.AccountNumber);
            document.getElementById('DisplayAccountNumberID').textContent = acc.accountNumber;
            document.getElementById('DisplayBalanceID').textContent = acc.balance;
            document.getElementById('DisplayUserNameID').textContent = acc.holderName;
            document.getElementById('DisplayEmailAddressID').textContent = acc.email;
            document.getElementById('DisplayPhoneNumberID').textContent = acc.phoneNumber;
        })
        .catch(error => {
            console.error('Error fetching account summary:', error);
            displayError('Failed to load account summary. Please try again later.');
        });
}


function displayError(message) {
    const errorElement = document.createElement('p');
    errorElement.style.color = 'red';
    errorElement.textContent = message;
    document.getElementById('UserAccountSummaryPage').appendChild(errorElement);
}

function performProfile() {
    const apiUrl = 'api/profile/username/' + encodeURIComponent(data.UserName);

    fetch(apiUrl)
        .then(response => response.json())
        .then(user => {
            document.getElementById('DisplayUserNameID').textContent = user.userName;
            document.getElementById('DisplayEmailAddressID').textContent = user.email;
            document.getElementById('DisplayPhoneNumberID').textContent = String(user.phone);
        })
        .catch(error => {
            console.error('Error fetching profile data:', error);
        });
}

/* LoginPage */
function performAuth() {
    var name = document.getElementById('UsernameID').value;
    var password = document.getElementById('PasswordID').value;
    data = {
        UserName: name,
        PassWord: password
    };
    console.error(data);
    const apiUrl = '/api/user/auth';

    const headers = {
        'Content-Type': 'application/json', // Specify the content type as JSON if you're sending JSON data
    };

    const requestOptions = {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(data) // Convert data object to JSON string
    };

    fetch(apiUrl, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            // Handle the data from the API
            const jsonObject = data;
            if (jsonObject.login) {
                loadView("authview");
                enableUserHomeButtons();
            }
            else {
                loadView("error");
            }

        })
        .catch(error => {
            console.error('Error: ' + error.message);
        });
}

function loadTransactionHistory() {
    const startDate = document.getElementById('StartDateInputID').value;
    const endDate = document.getElementById('EndDateInputID').value;
    const username = encodeURIComponent(data.UserName);

    let url = `${API_Transaction_URL}/history/${username}`;
    if (startDate) url += `?startDate=${startDate}`;
    if (endDate) url += `${startDate ? '&' : '?'}endDate=${endDate}`;

    fetch(url)
        .then(response => response.json())
        .then(transactions => {
            const tableBody = document.getElementById('TransactionHistoryTableBody');
            tableBody.innerHTML = '';
            transactions.forEach(transaction => {
                const row = tableBody.insertRow();
                row.insertCell(0).textContent = new Date(transaction.transactionTime).toLocaleString();
                row.insertCell(1).textContent = transaction.transactionName;
                row.insertCell(2).textContent = `$${transaction.transactionAmount.toFixed(2)}`;
                row.insertCell(3).textContent = transaction.transactionType;
            });
        })
        .catch(error => {
            console.error('Error fetching transaction history:', error);
        });
}

/* MoneyTransferPage */
function transfer() {
    let recipientAccountNumber = document.getElementById('RecipientAccountNumberID').value;
    let amount = document.getElementById('AmountID').value;
    let description = document.getElementById('DescriptionID').value;

    // Client-side validation
    if (!recipientAccountNumber || !amount || !description) {
        alert("Please fill in all fields");
        return;
    }

    if (isNaN(amount) || parseFloat(amount) <= 0) {
        alert("Please enter a valid amount");
        return;
    }

    var transferRequest = {
        SenderUsername: data.UserName, // Assuming data object has the sender's username
        RecipientAccountNumber: parseInt(recipientAccountNumber),
        Amount: parseFloat(amount),
        Description: description
    };

    fetch(`${API_Transfer_URL}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(transferRequest)
    })
        .then(response => {
            if (!response.ok) {
                return response.text().then(text => { throw new Error(text) });
            }
            return response.text();
        })
        .then(message => {
            alert("Transfer completed successfully.");
            // Optionally, refresh the account balance or redirect to account summary
            loadView('account');
        })
        .catch(error => {
            alert("Error: " + error.message);
        });
}

/* UserProfileInformationPage and (EditInformationPage - will go to this page & function) */
function updateUserInformation() {
    let newUserName = document.getElementById('UsernameID').value.trim();
    let newEmailAddress = document.getElementById('EmailID').value.trim();
    let newPhoneNumber = document.getElementById('PhoneNumberID').value.trim();
    let newPassword = document.getElementById('PasswordID').value;

    var updateRequest = {
        OldUsername: data.UserName,
        NewUsername: newUserName || null,
        Email: newEmailAddress || null,
        PhoneNumber: newPhoneNumber ? parseInt(newPhoneNumber) : null,
        NewPassword: newPassword || null
    };

    data.UserName = newUserName

    // Only include fields that have been changed
    Object.keys(updateRequest).forEach(key =>
        updateRequest[key] === null && delete updateRequest[key]
    );

    fetch(`${API_Editprofile_URL}/update`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(updateRequest)
    })
        .then(response => {
            if (!response.ok) {
                return response.text().then(text => { throw new Error(text) });
            }
            return response.text();
        })
        .then(message => {
            alert("User information updated successfully.");
            // Update the data object with the new information
            if (newUserName) data.UserName = newUserName;
            // Optionally, refresh the profile view or redirect to profile page
            loadView('profile');
        })
        .catch(error => {
            alert("Error: " + error.message);
        });
}

/* home page */
function enableUserHomeButtons() {
    const buttonIds = [
        'UserProfileInformationButtonID',
        'EditUserInformationButtonID',
        'UserAccountSummaryButtonID',
        'TransactionHistoryButtonID',
        'MoneyTransferID',
        'LogoutButtonID'
    ];

    buttonIds.forEach(id => {
        const button = document.getElementById(id);
        if (button) {
            button.disabled = false;
        }
    });
}

/* logout page */
function disableUserHomeButtons() {
    const buttonIds = [
        'UserProfileInformationButtonID',
        'EditUserInformationButtonID',
        'UserAccountSummaryButtonID',
        'TransactionHistoryButtonID',
        'MoneyTransferID',
        'LogoutButtonID'
    ];

    buttonIds.forEach(id => {
        const button = document.getElementById(id);
        if (button) {
            button.disabled = true;
        }
    });
}