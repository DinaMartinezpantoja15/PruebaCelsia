-- Active: 1723578531992@@blsmjzfcse5pxpjqcqp0-mysql.services.clever-cloud.com@3306@blsmjzfcse5pxpjqcqp0

CREATE TABLE Users (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    Name VARCHAR(255),
    Email VARCHAR(255) UNIQUE NOT NULL,
    Password VARCHAR(255) ,
    RegistrationDate DATETIME,
    Phone VARCHAR(20),
    Role VARCHAR(50),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    Address VARCHAR(255)
);

drop table `Invoices`

CREATE TABLE PaymentMethods (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    MethodName VARCHAR(50)
);


CREATE TABLE Invoices (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT,
    InvoicePeriod DATETIME,
    InvoiceNumber VARCHAR(50) UNIQUE,
    BilledAmount DECIMAL(10,2),
    PaidAmount DECIMAL(10,2), 
    Status VARCHAR(50),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

drop Table Invoices

DROP  TABLE Transactions

CREATE TABLE Transactions (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    InvoiceId INT,
    PaymentMethodId INT,
    CodigoTransacción VARCHAR(50),
    TransactionDate DATETIME,
    Amount DECIMAL(10,2),
    TransactionStatus VARCHAR(50),
    FOREIGN KEY (InvoiceId) REFERENCES Invoices(Id),
    FOREIGN KEY (PaymentMethodId) REFERENCES PaymentMethods(Id)
);


TRUNCATE TABLE Users;
