// Combined user dashboard and authentication functionality

document.addEventListener('DOMContentLoaded', function () {
    setupAuthenticationListeners();
    setupDashboardFunctionality();
});

// Authentication functions
function setupAuthenticationListeners() {
    const loginButton = document.getElementById('LoginButtonID');
    if (loginButton) {
        loginButton.addEventListener('click', performAuth);
    }

    const logoutButton = document.getElementById('LogoutButtonID');
    if (logoutButton) {
        logoutButton.addEventListener('click', logout);
    }
}

function performAuth() {
    var name = document.getElementById('UsernameID').value;
    var password = document.getElementById('PasswordID').value;
    var data = {
        UserName: name,
        PassWord: password
    };
    const apiUrl = '/api/login/auth';

    const headers = {
        'Content-Type': 'application/json',
    };

    const requestOptions = {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(data)
    };

    fetch(apiUrl, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            if (data.login) {
                enableDashboardButtons();
                loadView("LoginPage");
            } else {
                alert("User Authentication Process Failed. Please Try Again.");
            }
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function logout() {
    disableDashboardButtons();
    document.getElementById('content-container').innerHTML = '';
}

function enableDashboardButtons() {
    document.getElementById('LoginButtonID').disabled = true;
    document.getElementById('UserProfileInformationButtonID').disabled = false;
    document.getElementById('EditUserInformationButtonID').disabled = false;
    document.getElementById('UserAccountSummaryButtonID').disabled = false;
    document.getElementById('TransactionHistoryButtonID').disabled = false;
    document.getElementById('MoneyTransferID').disabled = false;
    document.getElementById('LogoutButtonID').disabled = false;
}

function disableDashboardButtons() {
    document.getElementById('LoginButtonID').disabled = false;
    document.getElementById('UserProfileInformationButtonID').disabled = true;
    document.getElementById('EditUserInformationButtonID').disabled = true;
    document.getElementById('UserAccountSummaryButtonID').disabled = true;
    document.getElementById('TransactionHistoryButtonID').disabled = true;
    document.getElementById('MoneyTransferID').disabled = true;
    document.getElementById('LogoutButtonID').disabled = true;
}

// User information functions
function userUpdatedInfoValidationCheck() {
    let username = document.getElementById('UsernameID');
    let email = document.getElementById('EmailID');
    let phoneNumber = document.getElementById('PhoneNumberID');
    let profilePicture = document.getElementById('ProfilePictureID');

    if (!username.value || !email.value || !phoneNumber.value || !profilePicture.files.length) {
        alert("Unable to Save Information. Please Fill All Fields.");
        return;
    }
    saveUserInformation();
}

function saveUserInformation() {
    // Implement the logic to save user information
    console.log("Saving user information...");
    // You can add AJAX call here to save the information to the server
}

// Dashboard functionality
function setupDashboardFunctionality() {
    loadUserInfo();
    loadAccountBalance();
    loadRecentTransactions();
    setupTransactionForm();
}

function loadUserInfo() {
    fetch('/Home/GetUserInfo')
        .then(response => response.json())
        .then(data => {
            document.getElementById('userName').textContent = data.userName;
            document.getElementById('userEmail').textContent = data.email;
        })
        .catch(error => console.error('Error loading user info:', error));
}

function loadAccountBalance() {
    fetch('/Account/GetBalance')
        .then(response => response.json())
        .then(data => {
            document.getElementById('accountBalance').textContent = `$${data.balance.toFixed(2)}`;
        })
        .catch(error => console.error('Error loading account balance:', error));
}

function loadRecentTransactions() {
    fetch('/Transaction/GetRecentTransactions')
        .then(response => response.json())
        .then(transactions => {
            const transactionList = document.getElementById('recentTransactions');
            transactionList.innerHTML = '';
            transactions.forEach(transaction => {
                const li = document.createElement('li');
                li.textContent = `${transaction.transactionTime}: ${transaction.transactionType} - $${transaction.transactionAmount.toFixed(2)}`;
                transactionList.appendChild(li);
            });
        })
        .catch(error => console.error('Error loading recent transactions:', error));
}

function setupTransactionForm() {
    const form = document.getElementById('transactionForm');
    if (form) {
        form.addEventListener('submit', function (e) {
            e.preventDefault();
            const formData = new FormData(form);
            fetch('/Transaction/MakeTransaction', {
                method: 'POST',
                body: formData
            })
                .then(response => response.json())
                .then(result => {
                    if (result.success) {
                        alert('Transaction successful!');
                        loadAccountBalance();
                        loadRecentTransactions();
                    } else {
                        alert('Transaction failed: ' + result.message);
                    }
                })
                .catch(error => console.error('Error making transaction:', error));
        });
    }
}

// Utility function (you might want to implement this)
function loadView(viewName) {
    console.log(`Loading view: ${viewName}`);
    // Implement the logic to load different views
}