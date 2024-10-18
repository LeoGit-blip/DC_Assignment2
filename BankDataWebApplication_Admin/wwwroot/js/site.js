// Base URL for API endpoints
const API_BASE_URL = '/api/admin';

function loadView(status) {
    let apiUrl = `${API_BASE_URL}`;

    switch (status) {
        case 'login':
            apiUrl = `${API_BASE_URL}/login`;
            break;
        case 'profile':
            apiUrl = `${API_BASE_URL}/profile`;
            break;
        case 'usermanagement':
            apiUrl = `${API_BASE_URL}/usermanagement`;
            break;
        case 'transactionmanagement':
            apiUrl = `${API_BASE_URL}/transactionmanagement`;
            break;
        case 'auditlogs':
            apiUrl = `${API_BASE_URL}/auditlogs`;
            break;
        case 'logout':
            apiUrl = `${API_BASE_URL}/logout`;
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
            initializeView(status);
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}

function initializeView(status) {
    switch (status) {
        case 'login':
            setupLoginForm();
            break;
        // Add other cases for different views 
    }
}

function setupLoginForm() {
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.addEventListener('submit', function (e) {
            e.preventDefault();
            performLogin();
        });
    }
}

function performLogin() {
    const username = document.getElementById('UsernameID').value;
    const password = document.getElementById('PasswordID').value;

    fetch(`${API_BASE_URL}/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ username, password })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                alert('Login successful!');
                enableDashboardButtons();
                loadView('profile');
            } else {
                alert('Login failed. Please check your credentials.');
            }
        })
        .catch(error => console.error('Error during login:', error));
}

function enableDashboardButtons() {
    const buttons = ['AdminProfileInformationID', 'UserManagementID', 'TransactionManagementID', 'LogsAndAuditTrailsID', 'AdminLogoutButtonID'];
    buttons.forEach(id => {
        const button = document.getElementById(id);
        if (button) {
            button.disabled = false;
        }
    });
    document.getElementById('LoginButtonID').disabled = true;
}

function handleLogout() {
    const buttons = ['AdminProfileInformationID', 'UserManagementID', 'TransactionManagementID', 'LogsAndAuditTrailsID', 'AdminLogoutButtonID'];
    buttons.forEach(id => {
        const button = document.getElementById(id);
        if (button) {
            button.disabled = true;
        }
    });
    document.getElementById('LoginButtonID').disabled = false;
    document.getElementById('main').innerHTML = '';
    loadView('login');
}