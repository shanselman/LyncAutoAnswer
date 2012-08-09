Lync 2012 Super Simple Auto Answer Video Kiosk with Full Screen

UI Suppression Edition
---------------------------------------------------------------

What it does
------------
Monitors the presence of a user (not the signed in user). Auto-answers and full-screens incoming video calls made to the signed-in user.

How to use it
-------------
1. Lync 2010 should be installed, but not running.
2. UISupressionMode should be ON (more information at LyncAutoAnswer.com)
3. Edit the app.config file:
	- change the value of sipEmailAddress to be the address you wish to monitor presence for.
	- change the LyncAccountDomainUser, LyncAccountEmail, LyncAccountPassword values to reflect the user you wish to sign in as.
	
	** This is a sample project. You should follow security best practice if you wish to use this in production. Storing user authentication information **
	** in plaintext is a security risk. Fork the project and implement your own security requirements. 													 **

More Information
----------------
The project repository: https://github.com/shanselman/LyncAutoAnswer
The project page: http://LyncAutoAnswer.com (full description, blog post references, author information etc.)