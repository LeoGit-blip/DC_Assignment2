// Base URL for API endpoints
const API_BASE_URL = '/api/admin';
const API_Logout_URL = '/api/logout';
const API_Profile_URL = '/api/adminprofile';
const API_User_URL = '/api/usermanagement';
const API_Transaction_URL = '/api/transactionmanagement';
const API_Log_URL = '/api/logs'

var data;

/* LoadView */
function loadView(status) {
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
        case 'usermanagement':
            apiUrl = `${API_User_URL}`;
            break;
        case 'transactionmanagement':
            apiUrl = `${API_Transaction_URL}`;
            break;
        case 'auditlogs':
            apiUrl = `${API_Log_URL}`;
            break;
        case 'logout':
            apiUrl = `${API_Logout_URL}`;
            disableAdminButtons()
            break;
        case 'error':
            apiUrl = `${API_BASE_URL}/error`;
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
            if (status == 'profile') {
                performAdminProfile();
            }
            else if (status == 'auditlogs')
            {
                fetchLogs();
            }
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}
/* End LoadView */

/* User Management */
function searchUsers() {
    const criteria = document.getElementById('data').value;
    const value = document.getElementById('SearchInputID').value;
    const apiUrl = `${API_User_URL}/search?criteria=${encodeURIComponent(criteria)}&value=${encodeURIComponent(value)}`;

    fetch(apiUrl)
        .then(response => response.json())
        .then(users => {
            displayUsers(users);
        })
        .catch(error => {
            console.error('Error searching users:', error);
        });
}
function displayUsers(users) {
    const tbody = document.querySelector('#UserManagementTable tbody');
    tbody.innerHTML = '';
    users.forEach(user => {
        const row = `<tr>
            <td>${user.userName}</td>
            <td>${user.email}</td>
            <td>${user.phone}</td>
            <td>${user.address}</td>
        </tr>`;
        tbody.innerHTML += row;
    });
}
function showCreateUserForm() {
    document.getElementById('CreateNewUserSection').style.display = 'block';
    document.getElementById('EditNewUserSection').style.display = 'none';
    document.getElementById('DeactivateUserSection').style.display = 'none';
}
function showEditUserForm() {
    document.getElementById('CreateNewUserSection').style.display = 'none';
    document.getElementById('EditNewUserSection').style.display = 'block';
    document.getElementById('DeactivateUserSection').style.display = 'none';
}
function showDeactivateUserForm() {
    document.getElementById('CreateNewUserSection').style.display = 'none';
    document.getElementById('EditNewUserSection').style.display = 'none';
    document.getElementById('DeactivateUserSection').style.display = 'block';
}
function createUser() {
    const user = {
        userName: document.getElementById('CreateUsernameID').value,
        email: document.getElementById('CreateEmailID').value,
        password: document.getElementById('CreatePasswordID').value,
        phone: document.getElementById('CreatePhoneNumberID').value,
        address: document.getElementById('CreateAddressID').value
    };

    fetch(`${API_User_URL}/create`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    })
        .then(response => response.json())
        .then(data => {
            alert(data.message);
            document.getElementById('CreateNewUserSection').style.display = 'none';
            searchUsers();
            logAction(`Created user: ${user.userName}`);
        })
        .catch(error => {
            console.error('Error creating user:', error);
        });
}
function editUser() {
    const username = document.getElementById('EditUsernameID').value;
    const user = {
        userName: username,
        email: document.getElementById('EditEmailID').value,
        password: document.getElementById('EditPasswordID').value,
        phone: document.getElementById('EditPhoneNumberID').value,
        address: document.getElementById('EditAddressID').value
    };

    fetch(`${API_User_URL}/edit/${encodeURIComponent(username)}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(user)
    })
        .then(response => response.json())
        .then(data => {
            alert(data.message);
            document.getElementById('EditNewUserSection').style.display = 'none';
            searchUsers();
            logAction(`Edited user: ${username}`)
        })
        .catch(error => {
            console.error('Error editing user:', error);
        });
}
function deactivateUser() {
    const username = document.getElementById('DeactivateUsernameID').value;

    fetch(`${API_User_URL}/deactivate/${encodeURIComponent(username)}`, {
        method: 'POST'
    })
        .then(response => response.json())
        .then(data => {
            alert(data.message);
            document.getElementById('DeactivateUserSection').style.display = 'none';
            searchUsers();
            logAction(`Deactivated user: ${username}`);
        })
        .catch(error => {
            console.error('Error deactivating user:', error);
        });
}
function resetPassword() {
    const username = prompt("Enter the username to reset password:");
    if (username) {
        fetch(`${API_User_URL}/reset-password/${encodeURIComponent(username)}`, {
            method: 'POST'
        })
            .then(response => response.json())
            .then(data => {
                alert(`${data.message}\nNew password: ${data.newPassword}`);
                logAction(`Reset password for user: ${username}`);
            })
            .catch(error => {
                console.error('Error resetting password:', error);
            });
    }
}
/* End User Management */

/* Transaction Management */
function loadTransactions() {
    fetch(`${API_BASE_URL}/transactionmanagement/all`)
        .then(response => response.json())
        .then(transactions => {
            displayTransactions(transactions);
        })
        .catch(error => {
            console.error('Error loading transactions:', error);
        });
}
function searchTransactions() {
    const criteria = document.getElementById('SearchDataOption').value;
    const value = document.getElementById('SearchInputID').value;
    const apiUrl = `api/transactionmanagement/search?criteria=${encodeURIComponent(criteria)}&value=${encodeURIComponent(value)}`;

    fetch(apiUrl)
        .then(response => response.json())
        .then(transactions => {
            displayTransactions(transactions);
        })
        .catch(error => {
            console.error('Error searching transactions:', error);
        });
}
function sortTransactions(sortBy) {
    const ascending = !document.getElementById(sortBy + 'SortIcon').classList.contains('asc');
    const apiUrl = `${API_BASE_URL}/transactionmanagement/sort?sortBy=${encodeURIComponent(sortBy)}&ascending=${ascending}`;

    fetch(apiUrl)
        .then(response => response.json())
        .then(transactions => {
            displayTransactions(transactions);
            updateSortIcon(sortBy, ascending);
        })
        .catch(error => {
            console.error('Error sorting transactions:', error);
        });
}
function displayTransactions(transactions) {
    const tbody = document.querySelector('#TransactionHistoryTable tbody');
    tbody.innerHTML = '';
    transactions.forEach(transaction => {
        const row = `<tr>
            <td>${new Date(transaction.transactionTime).toLocaleString()}</td>
            <td>${transaction.transactionName || 'N/A'}</td>
            <td>${transaction.transactionAmount.toFixed(2)}</td>
            <td>${transaction.transactionType || 'N/A'}</td>
            <td>${transaction.transactionID}</td>
        </tr>`;
        tbody.innerHTML += row;
    });
}
function updateSortIcon(sortBy, ascending) {
    const icons = document.querySelectorAll('.sort-icon');
    icons.forEach(icon => icon.classList.remove('asc', 'desc'));
    const icon = document.getElementById(sortBy + 'SortIcon');
    icon.classList.add(ascending ? 'asc' : 'desc');
}
/* End Transaction Management */

/* Profile */
function performAdminProfile() {
    const apiUrl = 'api/adminprofile/username/' + encodeURIComponent(data.UserName);

    fetch(apiUrl)
        .then(response => response.json())
        .then(user => {
            document.getElementById('DisplayAdminUserNameID').textContent = user.userName;
            document.getElementById('DisplayAdminEmailAddressID').textContent = user.email;
            document.getElementById('DisplayAdminPhoneNumberID').textContent = String(user.phone);

            // Populate edit fields
            document.getElementById('EditAdminUserNameID').value = user.userName;
            document.getElementById('EditAdminEmailAddressID').value = user.email;
            document.getElementById('EditAdminPhoneNumberID').value = String(user.phone);
        })
        .catch(error => {
            console.error('Error fetching profile data:', error);
        });
}
function toggleEditMode() {
    const displayElements = document.querySelectorAll('#AdminProfileInformationPage span');
    const editElements = document.querySelectorAll('#AdminProfileInformationPage input');
    const editButton = document.getElementById('EditButtonID');
    const saveButton = document.getElementById('SaveButtonID');

    displayElements.forEach(el => el.style.display = el.style.display === 'none' ? '' : 'none');
    editElements.forEach(el => el.style.display = el.style.display === 'none' ? '' : 'none');
    editButton.style.display = editButton.style.display === 'none' ? '' : 'none';
    saveButton.style.display = saveButton.style.display === 'none' ? '' : 'none';
}
function saveUserInformation() {
    const updatedUser = {
        userName: document.getElementById('EditAdminUserNameID').value,
        email: document.getElementById('EditAdminEmailAddressID').value,
        phone: document.getElementById('EditAdminPhoneNumberID').value
    };

    const apiUrl = 'api/adminprofile/update/' + encodeURIComponent(data.UserName);

    const headers = {
        'Content-Type': 'application/json', // Specify the content type as JSON if you're sending JSON data
        // Add any other headers you need here
    };

    const requestOptions = {
        method: 'PUT',
        headers: headers,
        body: JSON.stringify(updatedUser) // Convert the data object to a JSON string
    };

    fetch(apiUrl, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(dataOne => {
            console.log('Profile updated successfully:', dataOne);
            data.UserName = updatedUser.userName;
            toggleEditMode(); // Switch back to display mode
            performAdminProfile(); // Refresh the displayed information
        })
        .catch(error => {
            console.error('Error updating profile:', error);
        });
}

/* End Profile */

/* Logs and Audit Trails */
function fetchLogs() {
    const username = document.getElementById('usernameFilter').value;
    const action = document.getElementById('actionFilter').value;
    const fromDate = document.getElementById('fromDateFilter').value;
    const toDate = document.getElementById('toDateFilter').value;

    const queryParams = new URLSearchParams({
        username: username,
        action: action,
        fromDate: fromDate,
        toDate: toDate
    });

    fetch(`${API_Log_URL}/fetch?${queryParams}`)
        .then(response => response.json())
        .then(logs => {
            displayLogs(logs);
        })
        .catch(error => {
            console.error('Error fetching logs:', error);
        });
}
function displayLogs(logs) {
    const tbody = document.querySelector('#LogsAndAuditTrailsTable tbody');
    tbody.innerHTML = '';
    logs.forEach(log => {
        const row = `<tr>
            <td>${new Date(log.timestamp).toLocaleString()}</td>
            <td>${log.username}</td>
            <td>${log.action}</td>
        </tr>`;
        tbody.innerHTML += row;
    });
}
function logAction(action) {
    const logEntry = {
        username: data.UserName,
        action: action
    };

    fetch(`${API_Log_URL}/log`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(logEntry)
    })
        .then(response => response.json())
        .then(data => {
            console.log('Action logged:', data);
        })
        .catch(error => {
            console.error('Error logging action:', error);
        });
}

/* Login */
function performAuth() {

    var name = document.getElementById('UsernameID').value;
    var password = document.getElementById('PasswordID').value;
    data = {
        UserName: name,
        PassWord: password
    };
    console.error(data);
    const apiUrl = '/api/admin/auth';

    const headers = {
        'Content-Type': 'application/json', // Specify the content type as JSON if you're sending JSON data
        // Add any other headers you need here
    };

    const requestOptions = {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(data) // Convert the data object to a JSON string
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
                enableAdminButtons();
                logAction('Admin logged in');
            }
            else {
                loadView("error");
                logAction('Failed login attempt');
            }

        })
        .catch(error => {
            // Handle any errors that occurred during the fetch
            console.error('Fetch error:', error);
        });
}
function disableAdminButtons() {
    const buttonIds = [
        'AdminProfileInformationID',
        'UserManagementID',
        'TransactionManagementID',
        'LogsAndAuditTrailsID',
        'AdminLogoutButtonID'
    ];

    buttonIds.forEach(id => {
        const button = document.getElementById(id);
        if (button) {
            button.disabled = true;
        }
    });
}
function enableAdminButtons() {
    const buttonIds = [
        'AdminProfileInformationID',
        'UserManagementID',
        'TransactionManagementID',
        'LogsAndAuditTrailsID',
        'AdminLogoutButtonID'
    ];

    buttonIds.forEach(id => {
        const button = document.getElementById(id);
        if (button) {
            button.disabled = false;
        }
    });
}