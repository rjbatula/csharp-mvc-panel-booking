<h1 align="center"><bold>DEXTER'S LAB</bold></h1>
<hr>

<h2 align="center">Panel Booking System</h2>


<h3>Coding Workflow</h3>

<hr>

1. Clone the github repository to your dev environment
2. Create a branch from the master with naming convention [name]-[feature].
3. Fetch the latest from the master
4. Do your development work, stage and commit your changes with description
5. Create a pull request to this master branch for merge review

<h3>Features</h3>

<hr>

| F/No. |  Feature Name |  Assignee | Date of Completion | Status |
|---|---|---|---|---|
| DL-01 |  Base Skeleton in .NET |  Joshua | 3 Feb 2020 | Done |
| DL-02 |  Login System |  Joshua | 3 Feb 2020 | Done |
| DL-03 |  Registration |  Joshua | 3 Feb 2020 | Done |
|DL-04| User Profiling / Edit Profile | Joshua | 4 Feb 2020 | Done |
|DL-05| User Account Activation via Email | Joshua | 4 Feb 2020 | Done |
|DL-06| Password Recovery via Email | Joshua | 6 Feb 2020 | Done|
|DL-07| Create Panel Booking | Joshua | 12 Feb 2020 |Done|
|DL-08| View My Bookings | Joshua | 12 Feb 2020 |Done|
|DL-09| Edit Panel Booking | Joshua | 13 Feb 2020 |Done|
|DL-10| Delete Panel Booking | Joshua | 13 Feb 2020 |Done|
|DL-11| Show Booking at Home  | Joshua | 17 Feb 2020 |Done|
|DL-12| Button to Start Remote Desktop Connection  | Joshua | 18 Feb 2020 |Done|
|DL-13| Dynamic Credentials for RDC  | Joshua | 21 Feb 2020 |Done|
|DL-14| Responsive UI for App  | Joshua | 28 Feb 2020 |Done|
|DL-15| Button to Start SSH Connection  | Joshua | 4 Mar 2020 |Done|
|DL-16| Dyanamic Credentials for SSH  | Joshua | 6 Mar 2020 |Done|
|DL-17| CRUD SSH for Admin  | Joshua | 9 Mar 2020 |Done|


<h3>Assumptions</h3>

<hr>

1. We assumed that we know who is using the panels that were already in the room.
2. We Assumed that we know the purpose of testing for each device that were already in the room.

<h3>Devices</h3>

<hr>

| S/No. | Device Name |  Virtualisable? | Spaces Need in Web App |
|---|---|---|---|
| 1 |  Palo Alto Firewall |  Yes| 1|
| 2 |  ChecPoint Firewall |  Yes| 2|

<h3>Use Cases</h3>

<hr>

<h4>Login System</h4>

1. Users are able to create an account
2. Users are unable to login while their account is unactivated
3. Users are able to activate their account via email
4. Users are able to reset their password via email
5. Users are able to login and logout
6. Users are able to update their profile and password

<h4>Booking System</h4>

1. Users are able to see which panels were booked and not booked
2. Users are able to book their own panels as physical device and virtualised device
3. Users are able to edit their booking
4. Users are able to delete their booking


<h4>Remote Desktop Connection | SSH Connection</h4>

 1. Users are able to input their RDC credentials consisting of their IP, Domain, Username and Password at the Edit Booking page
 2. Users are able to see a RDC button once they have entered their credentials, changed server installed to "yes" for physical device and it is their booking date.
 3. Users are able to remote connect to their Virtual Machine once the button is pressed