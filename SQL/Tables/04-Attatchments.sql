CREATE TABLE attachments (
    attachment_id INT AUTO_INCREMENT PRIMARY KEY,
    attachment_name VARCHAR(255) NOT NULL,
    attachment_path VARCHAR(255) NOT NULL,
    attachment_type VARCHAR(255) NOT NULL,

    document_version_id INT NOT NULL,
    FOREIGN KEY (document_version_id) REFERENCES documents_versions(version_id)
    ON DELETE CASCADE
);