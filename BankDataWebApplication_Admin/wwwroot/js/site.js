// Base URL for API endpoints
const API_BASE_URL = '/api/admin';

function loadAdminProfile() {
    fetch(`${API_BASE_URL}/profile`)
        .then(response => response.json())
        .then(data => {
            document.getElementById('adminName').textContent = data.name;
            document.getElementById('adminEmail').textContent = data.email;
            document.getElementById('adminPhone').textContent = data.phone;
            if (data.profilePicture) {
                document.getElementById('adminProfilePic').src = data.profilePicture;
            }
        })
        .catch(error => console.error('Error loading admin profile:', error));
}

function setupUserManagement() {
    const userSearchForm = document.getElementById('userSearchForm');
    userSearchForm.addEventListener('submit', function (e) {
        e.preventDefault();
        const searchTerm = document.getElementById('userSearchInput').value;
        searchUsers(searchTerm);
    });

    document.getElementById('createUserBtn').addEventListener('click', createUser);
}

function searchUsers(searchTerm) {
    fetch(`${API_BASE_URL}/users/search?term=${encodeURIComponent(searchTerm)}`)
        .then(response => response.json())
        .then(users => {
            const userList = document.getElementById('userList');
            userList.innerHTML = '';
            users.forEach(user => {
                const userElement = document.createElement('div');
                userElement.innerHTML = `
                    <p>${user.name} (${user.email})</p>
                    <button onclick="editUser(${user.id})">Edit</button>
                    <button onclick="deactivateUser(${user.id})">Deactivate</button>
                    <button onclick="resetUserPassword(${user.id})">Reset Password</button>
                `;
                userList.appendChild(userElement);
            });
        })
        .catch(error => console.error('Error searching users:', error));
}

function setupTransactionManagement() {
    const transactionSearchForm = document.getElementById('transactionSearchForm');
    transactionSearchForm.addEventListener('submit', function (e) {
        e.preventDefault();
        const searchCriteria = {
            startDate: document.getElementById('startDate').value,
            endDate: document.getElementById('endDate').value,
            accountNumber: document.getElementById('accountNumber').value
        };
        searchTransactions(searchCriteria);
    });
}

function setupAuditLogs() {
    fetch(`${API_BASE_URL}/auditlogs`)
        .then(response => response.json())
        .then(logs => {
            const logList = document.getElementById('auditLogList');
            logList.innerHTML = '';
            logs.forEach(log => {
                const logElement = document.createElement('li');
                logElement.textContent = `${log.timestamp}: ${log.action} by ${log.adminName}`;
                logList.appendChild(logElement);
            });
        })
        .catch(error => console.error('Error loading audit logs:', error));
}

function createUser() {
    const userData = {
        name: document.getElementById('newUserName').value,
        email: document.getElementById('newUserEmail').value,
        phone: document.getElementById('newUserPhone').value
    };

    fetch(`${API_BASE_URL}/users`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userData)
    })
        .then(response => response.json())
        .then(data => {
            alert('User created successfully');
            searchUsers(''); // Refresh user list
        })
        .catch(error => console.error('Error creating user:', error));
}

function editUser(userId) {
    // Fetch user details and populate a form
    fetch(`${API_BASE_URL}/users/${userId}`)
        .then(response => response.json())
        .then(user => {
            document.getElementById('editUserName').value = user.name;
            document.getElementById('editUserEmail').value = user.email;
            document.getElementById('editUserPhone').value = user.phone;

            // Show edit form
            document.getElementById('editUserForm').style.display = 'block';

            // Setup form submission
            document.getElementById('editUserForm').onsubmit = function (e) {
                e.preventDefault();
                updateUser(userId);
            };
        })
        .catch(error => console.error('Error fetching user details:', error));
}

function updateUser(userId) {
    const userData = {
        name: document.getElementById('editUserName').value,
        email: document.getElementById('editUserEmail').value,
        phone: document.getElementById('editUserPhone').value
    };

    fetch(`${API_BASE_URL}/users/${userId}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userData)
    })
        .then(response => response.json())
        .then(data => {
            alert('User updated successfully');
            document.getElementById('editUserForm').style.display = 'none';
            searchUsers(''); // Refresh user list
        })
        .catch(error => console.error('Error updating user:', error));
}

function deactivateUser(userId) {
    if (confirm('Are you sure you want to deactivate this user?')) {
        fetch(`${API_BASE_URL}/users/${userId}/deactivate`, { method: 'POST' })
            .then(response => response.json())
            .then(data => {
                alert('User deactivated successfully');
                searchUsers(''); // Refresh user list
            })
            .catch(error => console.error('Error deactivating user:', error));
    }
}

function resetUserPassword(userId) {
    if (confirm('Are you sure you want to reset this user\'s password?')) {
        fetch(`${API_BASE_URL}/users/${userId}/reset-password`, { method: 'POST' })
            .then(response => response.json())
            .then(data => {
                alert(`Password reset successfully. New password: ${data.newPassword}`);
            })
            .catch(error => console.error('Error resetting password:', error));
    }
}

function searchTransactions(criteria) {
    const queryParams = new URLSearchParams(criteria);
    fetch(`${API_BASE_URL}/transactions/search?${queryParams}`)
        .then(response => response.json())
        .then(transactions => {
            const transactionList = document.getElementById('transactionList');
            transactionList.innerHTML = '';
            transactions.forEach(transaction => {
                const transactionElement = document.createElement('div');
                transactionElement.innerHTML = `
                    <p>Date: ${transaction.date}, Amount: $${transaction.amount}, 
                       Type: ${transaction.type}, Account: ${transaction.accountNumber}</p>
                `;
                transactionList.appendChild(transactionElement);
            });
        })
        .catch(error => console.error('Error searching transactions:', error));
}

// Initial setup
document.addEventListener('DOMContentLoaded', function () {
    loadAdminProfile();
    setupUserManagement();
    setupTransactionManagement();
    setupAuditLogs();
});