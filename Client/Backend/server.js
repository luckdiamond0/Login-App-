const express = require('express');
const bodyParser = require('body-parser');
const fs = require('fs');
const cors = require('cors');

const app = express();
const port = 3000;

app.use(bodyParser.json());
app.use(cors());

// Test route to check if the server is running
app.get('/', (req, res) => {
    res.send('Server is up and running.');
});

// Route to receive data from the client
app.post('/saveUserData', (req, res) => {
    const newUser = req.body;

    // Read the existing file or create an empty list if the file does not exist
    fs.readFile('userdata.json', (err, data) => {
        let users = [];

        if (!err) {
            try {
                users = JSON.parse(data);
            } catch (e) {
                console.error('Error parsing existing JSON data:', e);
            }
        }

        // Add the new user to the existing list
        users.push(newUser);

        // Write the updated list back to the file
        fs.writeFile('userdata.json', JSON.stringify(users, null, 2), (err) => { // Adding `null, 2` for readable formatting
            if (err) {
                console.error('Error saving data:', err);
                res.status(500).send('Error saving data');
            } else {
                console.log('Data received and saved successfully!');
                res.status(200).send('Data received and saved successfully!');
            }
        });
    });
});

// Start the server
app.listen(port, () => {
    console.log(`Server listening on port ${port}`);
});
