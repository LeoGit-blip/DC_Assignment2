document.addEventListener('DOMContentLoaded', function () {
    loadUserInfo();
    loadAccountBalance();
    loadRecentTransactions();
    setupTransactionForm();
});

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
                    loadAccountBalance(); // Refresh balance
                    loadRecentTransactions(); // Refresh transaction list
                } else {
                    alert('Transaction failed: ' + result.message);
                }
            })
            .catch(error => console.error('Error making transaction:', error));
    });
}