document.addEventListener('DOMContentLoaded', function () {
    loadDashboardStats();
    loadUserList();
    loadAccountList();
    loadTransactionChart();
});

function loadDashboardStats() {
    fetch('/Home/GetDashboardStats')
        .then(response => response.json())
        .then(data => {
            document.getElementById('totalUsers').textContent = data.totalUsers;
            document.getElementById('totalAccounts').textContent = data.totalAccounts;
            document.getElementById('totalTransactions').textContent = data.totalTransactions;
            document.getElementById('totalBalance').textContent = `$${data.totalBalance.toFixed(2)}`;
        })
        .catch(error => console.error('Error loading dashboard stats:', error));
}

function loadUserList() {
    fetch('/User/GetAllUsers')
        .then(response => response.json())
        .then(users => {
            const userList = document.getElementById('userList');
            userList.innerHTML = '';
            users.forEach(user => {
                const li = document.createElement('li');
                li.textContent = `${user.userName} (${user.email})`;
                userList.appendChild(li);
            });
        })
        .catch(error => console.error('Error loading user list:', error));
}

function loadAccountList() {
    fetch('/Account/GetAllAccounts')
        .then(response => response.json())
        .then(accounts => {
            const accountList = document.getElementById('accountList');
            accountList.innerHTML = '';
            accounts.forEach(account => {
                const li = document.createElement('li');
                li.textContent = `Account #${account.accountNumber}: $${account.balance.toFixed(2)}`;
                accountList.appendChild(li);
            });
        })
        .catch(error => console.error('Error loading account list:', error));
}

function loadTransactionChart() {
    fetch('/Transaction/GetTransactionSummary')
        .then(response => response.json())
        .then(data => {
            const ctx = document.getElementById('transactionChart').getContext('2d');
            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: data.labels,
                    datasets: [{
                        label: '# of Transactions',
                        data: data.values,
                        backgroundColor: 'rgba(75, 192, 192, 0.6)'
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        })
        .catch(error => console.error('Error loading transaction chart:', error));
}