# Transportation Management System

## Overview
This Transportation Management System (TMS) is designed to manage journeys and buses, facilitating efficient transportation management across three different systems: Admin, Manager, and User. Each system has its own front-end and API functions, ensuring a seamless and secure user experience.

## Table of Contents
- [Overview](#overview).
- [System Architecture](#system-architecture).
- [Analysis & Design Phase](#analysis--design-phase).
  - [Business Analysts](#business-analysts).
  - [Design](#Design).
- [Implementation Phase](#implementation-phase).
  - [Frontend (Web App)](#frontend-web-app).
  - [Backend (ASP.NET)](#backend-aspnet).
  - [Frontend (User View - Flutter)](#frontend-user-view-flutter).
- [Testing Phase](#testing-phase).
- [Project Repositories](#project-repositories).
- [Project Contributors](#project-contributors).
- [Reporting and Analysis Phase ](#Reporting-and-Analysis-Phase).
   - [Data Warehouse Desgin and Implementation](#Data-Warehouse-Desgin-and-Implementation).
   - [Dashboard](#Dashboard) 
  
## System Architecture
The TMS consists of three systems, each with specific functionalities and user interfaces:

1. **Admin System**
   - **Frontend**: Angular
   - **Backend**: RESTful APIs with authorization
   - **Functions**: User management, dashboard functionalities, and system settings

2. **Manager System**
   - **Frontend**: Angular
   - **Backend**: RESTful APIs with authorization
   - **Functions**: Journey creation, ticket management, and reporting

3. **User System**
   - **Frontend**: Flutter
   - **Backend**: RESTful APIs
   - **Functions**: User sign-up, ticket booking, and journey tracking

## Analysis & Design Phase

### Business Analysts
- [Mohamed Teba - ]Abd El-Rahman Darwish: Responsible for gathering and analyzing requirements for the admin dashboard.
- [Abd El-Rahman Darwish]: Focused on the warehouse management system, ensuring efficient inventory tracking and asset management.

### Design
- **System Design**: Create high-level design diagrams, including system architecture and data flow diagrams.
- **Database Design**: Design the database schema, including tables and relationships.
- **UI/UX Design**: Develop wireframes and prototypes for the front-end interfaces.
- **API Design**: Define the API endpoints, request/response formats, and authentication methods.

#### Contributors
- **Business Analyst**: [Abd El-Rahman Darwish](https://github.com/abdoDarwish)
- **System Designer**: [Mohamed Abd El-Maged]() - [Mohamed Teba]()
- **Database Designer**: [Abd El-Rahman Darwish](https://github.com/abdoDarwish)
- **UI/UX Designer**: [Amr Shalaby](https://github.com/amr1234567) - [Mohamed El-Sayed](https://github.com/mhmdelsyd) - [Shahd El-katsha](https://github.com/ShahdElkatsha) - [Rawan Yahia](https://github.com/rawanyahia11)

## Implementation Phase

### Frontend (Web App)

#### Admin System
- **Framework**: Angular
- **Components**: Develop components for user management and dashboard functionalities.
- **Services**: Create Angular services for communicating with backend APIs.
- **Authentication**: Implement JWT-based authentication.

#### Manager System
- **Framework**: Angular
- **Components**: Develop components for journey creation and ticket management.
- **Services**: Create Angular services for communicating with backend APIs.
- **Authentication**: Implement JWT-based authentication.

### Backend (ASP.NET)
- **Framework**: ASP.NET Core
- **API Development**: Implement RESTful APIs for Admin and Manager systems.
- **Database Integration**: Integrate the backend with the database to manage data.
- **Authentication**: Implement role-based access control and JWT authentication.
- **Business Logic**: Develop business logic for user management, journey creation, and ticket management.

### Frontend (User View - Flutter)
- **Framework**: Flutter
- **Components**: Develop components for user sign-up, ticket booking, and journey tracking.
- **Services**: Create Flutter services for communicating with backend APIs.
- **Authentication**: Implement token-based authentication.

#### Contributors
- **Frontend Developers (Web)**: [Amr Shalaby](https://github.com/amr1234567)
- **Backend Developers**: [Amr Shalaby](https://github.com/amr1234567) - [Mohamed El-Sayed](https://github.com/mhmdelsyd)
- **Frontend Developers (Flutter)**: [Shahd El-katsha](https://github.com/ShahdElkatsha) - [Rawan Yahia](https://github.com/rawanyahia11)

## Testing Phase

### Unit Testing
-Test individual services in API endpoints.

### Security Testing
- **Vulnerability Assessment**: Identify and fix security vulnerabilities in the application.
- **Penetration Testing**: Simulate attacks to test the system's defenses.

#### Contributors
- **Test Engineers**: [Amr Shalaby](https://github.com/amr1234567)
  
## Reporting and Analysis Phase 
### Data Warehouse Desgin and Implementation
- Data Warehouse was desgined based on the requirements.
- Data Warehouse was Implemented with SQL.
- SSIS Used For ETL.

### Dashboard 
- Power BI used for Creating dynamic dashboard that shows information based on the requirements.
- DAX Language used for Analysis and creating Measures.

  #### Contributors
  - **Data Analyst and BI Developer**: [Abd El-Rahman Darwish](https://github.com/abdoDarwish)

## Project Repositories
- **Admin System Frontend**: (https://github.com/amr1234567/Transportation.System_Admin_Frontend_Angular.git)
- **Manager System Frontend**: (https://github.com/amr1234567/Transportation.System_Manager_Frontend_Angular.git)
- **User System Frontend**: (https://github.com/ShahdElkatsha/transportation.git)
- **Backend**: (https://github.com/amr1234567/Transportation.System_BackEnd_Asp.Net.git)
- **Testing**: (https://github.com/amr1234567/Testing-Transportation-System.git)
- **Reporting And Analysis**: (https://github.com/abdoDarwish/MyProjects/tree/main/Transportation%20Managment%20System)
