INSERT INTO attachments (attachment_name, attachment_path, attachment_type, document_version_id)
VALUES
    (
        'john-smith-passport-front.jpg',
        '/uploads/john-smith/passport/v1/john-smith-passport-front.jpg',
        'image/jpeg',
        (
            SELECT dv.version_id
            FROM documents_versions dv
            INNER JOIN documents d ON dv.document_id = d.document_id
            WHERE d.title = 'John Smith Passport'
              AND dv.version_number = 1
            LIMIT 1
        )
    ),
    (
        'john-smith-passport-back.jpg',
        '/uploads/john-smith/passport/v1/john-smith-passport-back.jpg',
        'image/jpeg',
        (
            SELECT dv.version_id
            FROM documents_versions dv
            INNER JOIN documents d ON dv.document_id = d.document_id
            WHERE d.title = 'John Smith Passport'
              AND dv.version_number = 1
            LIMIT 1
        )
    ),
    (
        'john-smith-passport-bio-page.jpg',
        '/uploads/john-smith/passport/v1/john-smith-passport-bio-page.jpg',
        'image/jpeg',
        (
            SELECT dv.version_id
            FROM documents_versions dv
            INNER JOIN documents d ON dv.document_id = d.document_id
            WHERE d.title = 'John Smith Passport'
              AND dv.version_number = 1
            LIMIT 1
        )
    ),
    (
        'john-smith-visa.pdf',
        '/uploads/john-smith/visa/v1/john-smith-visa.pdf',
        'application/pdf',
        (
            SELECT dv.version_id
            FROM documents_versions dv
            INNER JOIN documents d ON dv.document_id = d.document_id
            WHERE d.title = 'John Smith Visa'
              AND dv.version_number = 1
            LIMIT 1
        )
    ),
    (
        'john-smith-visa-entry-stamp.jpg',
        '/uploads/john-smith/visa/v1/john-smith-visa-entry-stamp.jpg',
        'image/jpeg',
        (
            SELECT dv.version_id
            FROM documents_versions dv
            INNER JOIN documents d ON dv.document_id = d.document_id
            WHERE d.title = 'John Smith Visa'
              AND dv.version_number = 1
            LIMIT 1
        )
    ),
    (
        'john-smith-employment-agreement.pdf',
        '/uploads/john-smith/employment/v1/john-smith-employment-agreement.pdf',
        'application/pdf',
        (
            SELECT dv.version_id
            FROM documents_versions dv
            INNER JOIN documents d ON dv.document_id = d.document_id
            WHERE d.title = 'John Smith Employment Agreement'
              AND dv.version_number = 1
            LIMIT 1
        )
    ),
    (
        'john-smith-passport-renewed-scan.pdf',
        '/uploads/john-smith/passport/v2/john-smith-passport-renewed-scan.pdf',
        'application/pdf',
        (
            SELECT dv.version_id
            FROM documents_versions dv
            INNER JOIN documents d ON dv.document_id = d.document_id
            WHERE d.title = 'John Smith Passport'
              AND dv.version_number = 2
            LIMIT 1
        )
    ),
    (
        'john-smith-passport-renewed-photo.jpg',
        '/uploads/john-smith/passport/v2/john-smith-passport-renewed-photo.jpg',
        'image/jpeg',
        (
            SELECT dv.version_id
            FROM documents_versions dv
            INNER JOIN documents d ON dv.document_id = d.document_id
            WHERE d.title = 'John Smith Passport'
              AND dv.version_number = 2
            LIMIT 1
        )
    ),
    (
        'maria-garcia-passport-front.jpg',
        '/uploads/maria-garcia/passport/v1/maria-garcia-passport-front.jpg',
        'image/jpeg',
        (
            SELECT dv.version_id
            FROM documents_versions dv
            INNER JOIN documents d ON dv.document_id = d.document_id
            WHERE d.title = 'Maria Garcia Passport'
              AND dv.version_number = 1
            LIMIT 1
        )
    ),
    (
        'maria-garcia-passport-back.jpg',
        '/uploads/maria-garcia/passport/v1/maria-garcia-passport-back.jpg',
        'image/jpeg',
        (
            SELECT dv.version_id
            FROM documents_versions dv
            INNER JOIN documents d ON dv.document_id = d.document_id
            WHERE d.title = 'Maria Garcia Passport'
              AND dv.version_number = 1
            LIMIT 1
        )
    ),
    (
        'maria-garcia-visa.pdf',
        '/uploads/maria-garcia/visa/v1/maria-garcia-visa.pdf',
        'application/pdf',
        (
            SELECT dv.version_id
            FROM documents_versions dv
            INNER JOIN documents d ON dv.document_id = d.document_id
            WHERE d.title = 'Maria Garcia Visa'
              AND dv.version_number = 1
            LIMIT 1
        )
    ),
    (
        'maria-garcia-stcw-certificate.pdf',
        '/uploads/maria-garcia/stcw/v1/maria-garcia-stcw-certificate.pdf',
        'application/pdf',
        (
            SELECT dv.version_id
            FROM documents_versions dv
            INNER JOIN documents d ON dv.document_id = d.document_id
            WHERE d.title = 'Maria Garcia STCW Certificate'
              AND dv.version_number = 1
            LIMIT 1
        )
    ),
    (
        'maria-garcia-visa-renewed.pdf',
        '/uploads/maria-garcia/visa/v2/maria-garcia-visa-renewed.pdf',
        'application/pdf',
        (
            SELECT dv.version_id
            FROM documents_versions dv
            INNER JOIN documents d ON dv.document_id = d.document_id
            WHERE d.title = 'Maria Garcia Visa'
              AND dv.version_number = 2
            LIMIT 1
        )
    ),
    (
        'maria-garcia-visa-renewed-stamp.jpg',
        '/uploads/maria-garcia/visa/v2/maria-garcia-visa-renewed-stamp.jpg',
        'image/jpeg',
        (
            SELECT dv.version_id
            FROM documents_versions dv
            INNER JOIN documents d ON dv.document_id = d.document_id
            WHERE d.title = 'Maria Garcia Visa'
              AND dv.version_number = 2
            LIMIT 1
        )
    );
