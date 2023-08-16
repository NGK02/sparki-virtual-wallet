
Sparki Virtual Wallet
====================


>Sparkie is a virtual wallet web app that lets you take charge of your budget! You can send and receive money to everyone, deposit cash from your credit or debit card, and even exchange currencies with real-time rates from a 3rd party API.
 A solution that empowers users to effortlessly monitor their exchanges,transactions and transfers.Acess to tools which helps you make informed financial decisions.
 The project is developed using the ASP.NET Core 6 framework with a Custom User Interface built with MVC and TailWind CSS also public API's, ensuring a seamless and intuitive user experience.

## Project Description
### Key Features:
* **Email verification** -  Secure your virtual wallet with email verification. It’s quick, easy, and adds an extra layer of protection to your account.
* **Currency exchange** -  Sparki supports multiple currencies in your wallet. You can instantly exchange between your currencies with real dynamic exchange rates from a 3rd party API!
* **Comprehensive Transfer Records** - Options to sort your transactions based on your preferences, allowing you to quickly locate specific details and streamline your financial analysis.
* **Multiple cards** - With us you can easily manage all your debit and credit cards in one convenient place. Simply select the one you want to use within the app and you’re good to go.
* **Responsive design** - You can use our website on any device! We support multiple screen sizes from monitors, through tablets down to the smallest of smartphones!
* **Dark theme** - The dark theme is perfect for those who prefer a more subdued and elegant look and it's easier on the eyes. Plus, it can help save battery life on your device.
* **Refer a frined** - A fantastic opportunity for you to invite a friend and both of you enjoy a delightful gift.

#### Home Page
* The home page of our forum system showcases real-time active user count, total post count, and tables featuring the top ten most commented and created posts, providing visitors with a glimpse into the vibrant community and engaging discussions.

![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/homepage.jpeg)


* **Additionally, anonymous visitors have the privilege to read and view posts, enabling them to access valuable information. However, they are restricted from creating posts, commenting on existing posts, liking or disliking content, and accessing other users' profiles.**


![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/post.png)

#### Login Page
* The login page provides visitors with the opportunity to access the full potential of the website by logging in with their accounts, unlocking various features and functionalities.

![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/login.png)


### Register Page
* Visitors can register for the system and unlock the full potential o the website.

![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/Register.png)

### View All Posts
* In this forum system, every visitor of the website is provided with the ability to access and view all created posts. Additionally, visitors have the option to utilize the search feature to refine their browsing experience by searching for posts based on the following criteria:

     * Title - By entering specific keywords or phrases into the search bar, all posts that contain the input in their titles will be displayed. 
     * Content – By entering specific keywords or phrases into the search bar,all posts that contain the input in their content will be displayed.
     * Created by - By entering a specific username as input, all posts that match the input and were created by the corresponding user will be displayed.
     * Tag - By entering relevant keywords or phrases as input, all posts that contain matching tags will be displayed. 
	 * Start/End Date - By selecting a start date and an end date, the system will retrieve and display all posts that fall within that date range.
	 * Sort posts by:
	  	 * Title
		 * Comments
		 * Likes
		 * Date
	* Order posts in either ascending or descending order.
		<br><br>
	![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/ViewAllPosts.png)
### Main Search for NOT logged users
* Anonymous visitors are granted access to the search functionality available in the navigation bar. This feature allows them to search for posts based on specific keywords present in the post title, as well as filter results by relevant tags.
	<br><br>
	![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/Search.png)
* Results
	<br><br>
	![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/NOTloggedResults.png)
	
### Main Search for logged users
* Once a visitor is authenticated, they gain access to the main search feature, which enables them to search for other users based on specific keywords present in their profile title.
	<br><br>
	![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/LoggedResults.png)
### Create Post 
* Logged-in users have the ability to create posts within the forum system.
	<br><br>
	![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/CreatePost.png)
### Edit Post
* Furthermore, logged-in users who are the creators of a post possess the capability to make edits to the post, including modifying the title, content, and adding or modifying tags associated with the post.
	<br><br>
	![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/EditPost.png)
### Add Comment
* Logged-in users are provided with the privilege to actively engage with the community by adding comments to existing posts.
	<br><br>
    ![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/AddComment.png)
### Tag Details
* If a logged-in user is either the creator of a tag or an admin, they have the additional privileges of deleting or editing the tag. Furthermore, if a logged-in user is the owner of a post or an admin, they have the authority to remove existing tags associated with that particular post.
	<br><br>
    ![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/TagDetails.png)
## Administrative part
* When an admin is logged into the system, they gain access to the Admin panel, unlocking a range of administrative functionalities. Within the Admin panel, admins are empowered to search for individuals based on their first name, username, or email address.
	<br><br>
    ![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/AdminPanel.png)
	<br><br>
* From the Admin panel, administrators have comprehensive access to user profiles, allowing them to view detailed user information. Additionally, administrators are provided with several options to manage user accounts effectively, including the ability to edit user profiles, block or unblock accounts, delete user accounts, or promote users to the admin role.
	<br><br>
    ![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/Admin.png)
	<br><br>
* When the Admin utilizes the "View All Posts" functionality, they are presented with an option to directly delete posts from the search results.
	<br><br>
    ![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/AdminPosts.png)
	<br><br>
## API
* The Blockie forum system provides API documentation in Swagger format, allowing developers to easily explore and understand the available endpoints, request/response structures, and supported operations. The Swagger documentation provides a user-friendly interface where developers can interactively test API endpoints, view sample requests and responses, and access detailed descriptions of each API resource. 
    ![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/API.png)
## Database Diagram
* A detailed database diagram is included to provide a comprehensive and objective view of the underlying data structure in the Blockie forum system. This diagram visually represents the relationships between different database tables and the structure of the data model. It offers insights into the organization of user data, posts, comments, tags, and other relevant entities within the system. 
    ![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/DatabaseDiagram.png)

## Additionally
* In-memory database, MSTest and Moq for testing
* Data transfer objects(DTOs)
* Auto-Mapper
* Above 80% Unit test code coverage of the business logic
* Public API's
* Technologies
     * ASP.NET Core
	 * Entity Framework Core
	 * Mock Framework
	 * MS SQL Server
	 * HTML
	 * CSS
## Team Members
* Atanas Iliev - [GitLab](https://gitlab.com/atanasiliev1293)
* Nikolai Gigov - [GitLab](https://gitlab.com/NG02)
* Katrin Lilova - [GitLab](https://gitlab.com/katrinlilova)
* Telerik Acedemy Official Project
![Alt text](https://gitlab.com/project-one-group-five/forum-system/-/raw/main/ImagesForREADME/telerik.PNG)