INSERT INTO documents_versions (version_number, expiration_date, document_id, last_modified_by)
VALUES
    (
        1,
        DATE_ADD(NOW(), INTERVAL 1 YEAR),
        (SELECT document_id FROM documents WHERE title = 'John Smith Passport' LIMIT 1),
        (SELECT user_id FROM users WHERE email = 'eddy.ng@sq.example.com' LIMIT 1)
    ),

    (
        1,
        DATE_ADD(NOW(), INTERVAL 1 YEAR),
        (SELECT document_id FROM documents WHERE title = 'John Smith Visa' LIMIT 1),
        (SELECT user_id FROM users WHERE email = 'andrew.lauwira@sq.example.com' LIMIT 1)
    ),

    (
        1,
        DATE_ADD(NOW(), INTERVAL 1 YEAR),
        (SELECT document_id FROM documents WHERE title = 'John Smith Employment Agreement' LIMIT 1),
        (SELECT user_id FROM users WHERE email = 'maria.santos@sq.example.com' LIMIT 1)
    ),

    (
        2,
        DATE_ADD(NOW(), INTERVAL 5 YEAR),
        (SELECT document_id FROM documents WHERE title = 'John Smith Passport' LIMIT 1),
        (SELECT user_id FROM users WHERE email = 'eddy.ng@sq.example.com' LIMIT 1)
    ),

    (
        1,
        DATE_ADD(NOW(), INTERVAL 5 YEAR),
        (SELECT document_id FROM documents WHERE title = 'Maria Garcia Passport' LIMIT 1),
        (SELECT user_id FROM users WHERE email = 'compliance.admin@sq.example.com' LIMIT 1)
    ),

    (
        1,
        DATE_ADD(NOW(), INTERVAL 2 YEAR),
        (SELECT document_id FROM documents WHERE title = 'Maria Garcia Visa' LIMIT 1),
        (SELECT user_id FROM users WHERE email = 'eddy.ng@sq.example.com' LIMIT 1)
    ),

    (
        1,
        DATE_ADD(NOW(), INTERVAL 1 YEAR),
        (SELECT document_id FROM documents WHERE title = 'Maria Garcia STCW Certificate' LIMIT 1),
        (SELECT user_id FROM users WHERE email = 'andrew.lauwira@sq.example.com' LIMIT 1)
    ),

    (
        2,
        DATE_ADD(NOW(), INTERVAL 2 YEAR),
        (SELECT document_id FROM documents WHERE title = 'Maria Garcia Visa' LIMIT 1),
        (SELECT user_id FROM users WHERE email = 'compliance.admin@sq.example.com' LIMIT 1)
    );
