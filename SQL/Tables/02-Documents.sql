CREATE TABLE documents (
    document_id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(255) NOT NULL,
    creation_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    crew_name VARCHAR(255) NOT NULL,

    created_by INT NOT NULL,
    FOREIGN KEY (created_by) REFERENCES users(user_id)
);