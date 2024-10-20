const API_BASE_URL = '/api/user';
const API_Logout_URL = '/api/logout';
const API_Profile_URL = '/api/userprofile';

/* Index.cstml */
function loadView() {
    let apiUrl = ${ API_BASE_URL };

    switch (status) {
        case 'login':
            apiUrl = ${ API_BASE_URL } /login;
            break;
        case 'authview':
            apiUrl = ${ API_BASE_URL } /authview;
            break;
        case 'profile':
            apiUrl = ${ API_Profile_URL };
            break;
        case 'editprofile':
            apiUrl = ${ API_BASE_URL } /editprofile;
            break;
        case 'transaction':
            apiUrl = ${ API_BASE_URL } /transaction;
            break;
        case 'logout':
            apiUrl = ${ API_Logout_URL };
            disableUserHomeButtons();
            break;
        case 'error':
            apiUrl = ${ API_BASE_URL } /error;
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
            document.getElementById('main').innerHTML = data;
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

/* LoginPage */

function performAuth() {
    var name = document.getElementById('UsernameID').value;
    var password = document.getElementById('PasswordID').value;
    var data = {
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

/* TransactionHistoryPage */
function filterInfo() {
    let searchedDate = document.getElementById('DateInputID').value;


}

/* MoneyTransferPage */
function transfer() {
    let recipientName = document.getElementById('ReceipientNameID').value;
    let amount = document.getElementById('AmountID').value;
    let date = document.getElementById('DateID').value;
    let description = document.getElementById('DescriptionID').value;

    if (!recipientName || !amount || !date || !description) {
        alert("Please")
    }

    var ongoingTransaction = {
        recepientName: recipientName,
        amount: parseFloat(amount),
        date: date,
        description: description;
    };

    fetch('/api/Transaction', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(ongoingTransaction) // Converting data object to JSON string
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok. An error has occured.');
            }
            return response.text(); // Assuming the server sends a text response upon success
        })
        .then(message => {
            alert("Transaction Complete....");
        })
        .catch(error => {
            alert("Error: " + error.message);
        });
}

/* UserProfileInformationPage and (EditInformationPage - will go to this page & function) */
function editUserInformation() {
    let newUserName = document.getElementById('EditUserNameID').value;
    let newEmailAddress = document.getElementById('EditEmailAddressID').value;
    let newPhoneNumber = document.getElementById('EditPhoneNumberID').value;
    let newPfp = document.getElementById('ProfilePictureID'); // image

    document.getElementById('DisplayUserNameID').innerText = newUserName;
    document.getElementById('DisplayEmailAddressID').innerText = newEmailAddress;
    document.getElementById('DisplayPhoneNumberID').innerText = newPhoneNumber;
    document.getElementById('DisplayProfilePictureID') = newPfp;

    if (newPfp.files && newPfp.files[0]) {
        var fileReader = new FileReader();
        fileReader.onload = function (e) {
            // set image
            document.getElementById('DisplayProfilePictureID').src = e.target.result;
        };

        fileReader.readAsDataURL(newPfp.files[0]);
    }
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