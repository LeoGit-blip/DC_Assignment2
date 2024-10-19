/* Index.cstml */
function loadView() {

}

/* LoginPage */

function performAuth() {

}

/* EditInformationPage */
function userUpdatedInfoValidationCheck() {

}

/* TransactionHistoryPage */
function filterInfo() {

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
            if (response.ok) {
                return response.text(); // Assuming the server sends a text response upon success
            } else {
                throw new Error('Network response was not ok. An error has occured.');
            }
        })
        .then(message => {
            alert("Transaction Complete....");
        })
        .catch(error => {
            alert("Error: ", error);
        });
}

/* UserProfileInformationPage */
function editUserInformation() {
    let newUserName = document.getElementById('EditUserNameID').value;
    let newEmailAddress = document.getElementById('EditEmailAddressID').value;
    let newPhoneNumber = document.getElementById('EditPhoneNumberID').value;

    document.getElementById('DisplayUserNameID').inn
}