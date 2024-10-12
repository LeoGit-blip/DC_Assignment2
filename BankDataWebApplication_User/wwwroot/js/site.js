// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
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
            if (data.login) // if successful authentication
            {
                document.getElementById('LoginButtonID').disabled = true; // disable the login button

                // enable buttons
                document.getElementById('UserProfileInformationButtonID').disabled = false;
                document.getElementById('EditUserInformationButtonID').disabled = false;
                document.getElementById('UserAccountSummaryButtonID').disabled = false;
                document.getElementById('TransactionHistoryButtonID').disabled = false;
                document.getElementById('MoneyTransferID').disabled = false;
                document.getElementById('LogoutButtonID').disabled = false;

                // Load the authenticated view or dashboard
                loadView("LoginPage");
            }
            else // Authentication failed
            {
                alert("User Authentication Process Failed. Please Try Again.");
            }
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}


function logout()
{
    // enable buttons
    document.getElementById('LoginButtonID').disabled = false; 

    // disable buttons
    document.getElementById('UserProfileInformationButtonID').disabled = true;
    document.getElementById('EditUserInformationButtonID').disabled = true;
    document.getElementById('UserAccountSummaryButtonID').disabled = true;
    document.getElementById('TransactionHistoryButtonID').disabled = true;
    document.getElementById('MoneyTransferID').disabled = true;
    document.getElementById('LogoutButtonID').disabled = true; 

    // Optionally clear the content container or redirect to home
    document.getElementById('content-container').innerHTML = '';
}