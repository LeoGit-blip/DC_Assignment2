// Base URL for API endpoints
const API_BASE_URL = '/api/admin';
const API_Logout_URL = '/api/logout';
const API_Profile_URL = '/api/adminprofile';

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
            apiUrl = `${API_BASE_URL}/usermanagement`;
            break;
        case 'transactionmanagement':
            apiUrl = `${API_BASE_URL}/transactionmanagement`;
            break;
        case 'auditlogs':
            apiUrl = `${API_BASE_URL}/auditlogs`;
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
        })
        .catch(error => {
            console.error('Fetch error:', error);
        });
}
function performAuth() {

    var name = document.getElementById('UsernameID').value;
    var password = document.getElementById('PasswordID').value;
    var data = {
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
            }
            else {
                loadView("error");
            }

        })
        .catch(error => {
            // Handle any errors that occurred during the fetch
            console.error('Fetch error:', error);
        });
}

function disableAdminButtons() {
    console.log("Buttons Enabled");
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