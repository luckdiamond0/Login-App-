document.getElementById('submit').addEventListener('click', () => {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;
    const key = document.getElementById('key').checked;

    const user = {
        Username: username,
        Password: password,
        Key: key
    };

    // Store the new user in localStorage
    let users = JSON.parse(localStorage.getItem('users')) || [];
    users.push(user);
    localStorage.setItem('userdata', JSON.stringify(users));

    // Send the data to the local server
    fetch('http://localhost:3000/saveUserData', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user) // Send only the new user
    })
    .then(response => {
        if (response.ok) {
            console.log('Data successfully sent to the server!');
        } else {
            console.error('Failed to send data to the server.');
        }
    })
    .catch(error => {
        console.error('Error sending data:', error);
    });
});

function showUsers() {
    let users = JSON.parse(localStorage.getItem('userdata'));
    console.log(users);
}

window.onload = showUsers;
