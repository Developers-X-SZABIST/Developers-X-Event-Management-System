Introduction
Event management is a critical task for organizations, institutions, and communities that regularly host seminars, workshops, conferences, and social gatherings. Managing events manually often leads to issues such as inconsistent data, overbooking, poor access control, and a lack of real-time updates. 
This project presents a Web-based Event Management System developed using ASP.NET Core 8 following the Model–View–Controller (MVC) architectural pattern. The system is designed to simplify the process of organizing events, managing registrations, and controlling access through role-based authorization. It supports multiple user roles, including Admin, Organizer, and Public users, each with clearly defined responsibilities and permissions.
The application enables public users to browse available events and register online, while administrators and organizers can manage events, users, and registrations through a secure interface. By automating event handling and enforcing business rules such as capacity limits and registration deadlines, the system ensures reliability, security, and ease of use.
























Project Overview
Purpose of the System
The primary goal of the Event Management System is to provide a centralized platform that:
Allows users to view and register for events online


Enables organizers to manage event registrations


Provides administrators with full control over users and events


Ensures secure access through authentication and role-based authorization


The system replaces manual or fragmented event handling with a structured and scalable digital solution.

System Architecture
The application follows the MVC (Model–View–Controller) architectural pattern:
Model
 Represents the core business data and rules, including entities such as User, Event, and Registration. Models interact with the database using Entity Framework Core.


View
 Responsible for the presentation layer. Views are implemented using Razor Pages and styled with Bootstrap, ensuring a responsive and user-friendly interface.


Controller
 Handles user requests, applies business logic, enforces role-based access, and communicates between models and views.


This separation of concerns improves maintainability, scalability, and testability of the system.



Technology Stack
The system is built using the following technologies:
Backend Framework:


ASP.NET Core 8


C# Programming Language


Architecture Pattern:


Model–View–Controller (MVC)


Database:


Microsoft SQL Server


SQL Server Management Studio (SSMS)


Data Access:


Entity Framework Core (Code-First approach)


Authentication & Security:


JWT (JSON Web Token) based authentication


Role-based authorization (Admin, Organizer, Public)


Frontend / UI:


Razor Views


Tailwind for responsive design








Key Features and Functionality
Public Users
View available events


View event details


Register for events (subject to seat availability and deadlines)


Deregister from previously registered events


Authentication required for registration


Organizers
View event registrations


Create, edit, and delete registrations


Manage event participation


Administrators
Full control over users


Create, update, and delete events


Manage registrations


Enforce system rules and access control


All features are protected using role-based authorization to ensure system security.







Functional Requirements
The system provides role-based event management, allowing public users to register for events, organizers to manage registrations, and administrators to manage users and events.

FR-1 User Authentication & Authorization
Users shall be able to register, log in, and log out.


The system assigns users one of three roles: Public, Organizer, or Admin.


Access to features shall be restricted using role-based authorization.



FR-2 Event Viewing (Public Users)
Public users shall be able to view a list of available events.


Public users shall be able to view event details.



FR-3 Event Registration (Public Users)
Public users shall be able to register for events.


Registration shall only be allowed if seats are available and the due date has not passed.


Users must be logged in before registering.


The system shall prevent duplicate registrations.


Users shall be able to deregister from events they have registered for.



FR-4 Event Management (Admin & Organizer)
Admins and organizers shall be able to create events.


Admins and organizers shall be able to edit event details.


Admins and organizers shall be able to delete events.


Admins and organizers shall be able to view all events.



FR-5 Registration Management (Admin & Organizer)
Admins and organizers shall be able to view event registrations.


Admins and organizers shall be able to edit registration details.


Admins and organizers shall be able to delete registrations.



FR-6 User Management (Admin Only)
Admins shall be able to view all users.


Admins shall be able to create, edit, and delete users.



FR-7 System Feedback
The system shall display confirmation or error messages for user actions.


The system shall update the event and registration status in real time.




Non-Functional Requirements
Overview:
The system ensures secure, reliable, and maintainable operation while following standard software engineering practices.

NFR-1 Security
Passwords shall be stored in hashed form.


The system shall use JWT-based authentication.


Unauthorized users shall be denied access to protected features.



NFR-2 Performance
The system shall support multiple concurrent users.


Event and registration data shall load within an acceptable response time.



NFR-3 Usability
The user interface shall be simple and easy to navigate.


Users shall receive clear feedback for successful and failed operations.



NFR-4 Reliability
The system shall ensure data consistency during registrations.


Invalid inputs shall be handled gracefully without system failure.



NFR-5 Maintainability
The system shall follow the MVC architectural pattern.


Code shall be modular and easy to extend.



NFR-6 Scalability
The system shall support future feature expansion (e.g., new roles or modules).


The database design shall support increasing data volume.



NFR-7 Portability
The system shall be deployable on any platform that supports .NET Core.


Configuration settings shall be environment-independent.







Database Design Overview
The system uses a relational database consisting of:
Users – stores user credentials and roles


Events – stores event details such as title, date, capacity, and deadline


Registrations – links users to events and tracks registration status


Relationships are enforced using foreign keys, ensuring data integrity and consistency.



Conclusion
The Event Management System demonstrates a complete, role-based web application using .NET Core MVC, enabling public users to register for events, organizers to manage registrations, and administrators to oversee users and events. 
The system follows a modular architecture, ensures data integrity, and provides secure authentication and authorization. 
Through clear functional and non-functional requirements, a well-structured database, and detailed activity diagrams, the project illustrates both the design and practical implementation of a scalable and maintainable software solution.

