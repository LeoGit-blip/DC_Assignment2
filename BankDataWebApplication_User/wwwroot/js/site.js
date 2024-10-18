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
    // Gather user information from the form
    const username = document.getElementById('UsernameID').value;
    const email = document.getElementById('EmailID').value;
    const phoneNumber = document.getElementById('PhoneNumberID').value;
    const profilePicture = document.getElementById('ProfilePictureID').files[0];

    // Create a FormData object to handle file upload
    const formData = new FormData();
    formData.append('username', username);
    formData.append('email', email);
    formData.append('phoneNumber', phoneNumber);
    formData.append('profilePicture', profilePicture);

    // AJAX call to save the information to the server
    fetch('/api/user/updateProfile', {
        method: 'POST',
        body: formData
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                alert('User information updated successfully!');
                // Optionally, you can update the UI here to reflect the changes
                updateUIWithNewUserInfo(data.user);
            } else {
                alert('Failed to update user information: ' + data.message);
            }
        })
        .catch(error => {
            console.error('Error saving user information:', error);
            alert('An error occurred while saving user information. Please try again.');
        });
}

function updateUIWithNewUserInfo(user) {
    // Update the UI elements with the new user information
    document.getElementById('userName').textContent = user.username;
    document.getElementById('userEmail').textContent = user.email;
    // Update other UI elements as needed
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

function loadView(viewName) {
    console.log(`Loading view: ${viewName}`);

    // Show loading indicator
    const contentContainer = document.getElementById('content-container');
    contentContainer.innerHTML = '<div class="loading">Loading...</div>';

    // Fetch the view content from the server
    fetch(`/Home/GetView?viewName=${viewName}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(html => {
            // Update the content container with the new view
            contentContainer.innerHTML = html;

            // Update the URL without reloading the page
            history.pushState(null, '', `/${viewName}`);

            // Update any active states in the navigation
            updateNavigation(viewName);

            // Initialize any scripts specific to this view
            initViewScripts(viewName);
        })
        .catch(error => {
            console.error('Error loading view:', error);
            contentContainer.innerHTML = '<div class="error">Error loading content. Please try again.</div>';
        });
}

function updateNavigation(viewName) {
    // Remove 'active' class from all nav items
    document.querySelectorAll('.nav-item').forEach(item => item.classList.remove('active'));

    // Add 'active' class to the current view's nav item
    const activeNavItem = document.querySelector(`.nav-item[data-view="${viewName}"]`);
    if (activeNavItem) {
        activeNavItem.classList.add('active');
    }
}

function initViewScripts(viewName) {
    switch (viewName) {
        case 'Dashboard':
            loadUserInfo();
            loadAccountBalance();
            loadRecentTransactions();
            break;
        case 'TransactionHistory':
            loadFullTransactionHistory();
            break;
        case 'UserProfile':
            loadUserProfileDetails();
            break;
        // Add cases for other views as needed
    }
}