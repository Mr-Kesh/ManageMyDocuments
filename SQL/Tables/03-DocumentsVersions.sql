CREATE TABLE documents_versions (
    version_id INT AUTO_INCREMENT PRIMARY KEY,
    version_number INT NOT NULL,
    expiration_date DATETIME NOT NULL,
    last_modified_time DATETIME DEFAULT CURRENT_TIMESTAMP,
    
    document_id INT NOT NULL,
    FOREIGN KEY (document_id) REFERENCES documents(document_id),

    last_modified_by INT NOT NULL,
    FOREIGN KEY(last_modified_by) REFERENCES users(user_id)
);