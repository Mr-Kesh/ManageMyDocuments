INSERT INTO documents (title, crew_name, created_by)
VALUES
    (
        'John Smith Passport',
        'John Smith',
        (SELECT user_id FROM users WHERE email = 'eddy.ng@sq.example.com' LIMIT 1)
    ),

    (
        'John Smith Visa',
        'John Smith',
        (SELECT user_id FROM users WHERE email = 'andrew.lauwira@sq.example.com' LIMIT 1)
    ),

    (
        'John Smith Employment Agreement',
        'John Smith',
        (SELECT user_id FROM users WHERE email = 'manager@sq.example.com' LIMIT 1)
    ),

    (
        'John Smith Vaccination Record',
        'John Smith',
        (SELECT user_id FROM users WHERE email = 'maria.santos@sq.example.com' LIMIT 1)
    ),

    (
        'Maria Garcia Passport',
        'Maria Garcia',
        (SELECT user_id FROM users WHERE email = 'compliance.admin@sq.example.com' LIMIT 1)
    ),

    (
        'Maria Garcia Visa',
        'Maria Garcia',
        (SELECT user_id FROM users WHERE email = 'eddy.ng@sq.example.com' LIMIT 1)
    ),

    (
        'Maria Garcia STCW Certificate',
        'Maria Garcia',
        (SELECT user_id FROM users WHERE email = 'andrew.lauwira@sq.example.com' LIMIT 1)
    ),

    (
        'Maria Garcia Medical Certificate',
        'Maria Garcia',
        (SELECT user_id FROM users WHERE email = 'david.chen@sq.example.com' LIMIT 1)
    ),

    (
        'Robert Johnson Passport',
        'Robert Johnson',
        (SELECT user_id FROM users WHERE email = 'sarah.johnson@sq.example.com' LIMIT 1)
    ),

    (
        'Robert Johnson Employment Agreement',
        'Robert Johnson',
        (SELECT user_id FROM users WHERE email = 'admin@sq.example.com' LIMIT 1)
    ),

    (
        'Robert Johnson Safety Training Certificate',
        'Robert Johnson',
        (SELECT user_id FROM users WHERE email = 'michael.lee@sq.example.com' LIMIT 1)
    ),

    (
        'Robert Johnson Vaccination Record',
        'Robert Johnson',
        (SELECT user_id FROM users WHERE email = 'linda.garcia@sq.example.com' LIMIT 1)
    ),

    (
        'Ana Rodriguez Passport',
        'Ana Rodriguez',
        (SELECT user_id FROM users WHERE email = 'robert.wilson@sq.example.com' LIMIT 1)
    ),

    (
        'Ana Rodriguez Visa',
        'Ana Rodriguez',
        (SELECT user_id FROM users WHERE email = 'compliance.admin@sq.example.com' LIMIT 1)
    ),

    (
        'Ana Rodriguez Crew Contract',
        'Ana Rodriguez',
        (SELECT user_id FROM users WHERE email = 'james.anderson@sq.example.com' LIMIT 1)
    ),

    (
        'Ana Rodriguez Medical Certificate',
        'Ana Rodriguez',
        (SELECT user_id FROM users WHERE email = 'patricia.martinez@sq.example.com' LIMIT 1)
    ),

    (
        'Michael Brown Passport',
        'Michael Brown',
        (SELECT user_id FROM users WHERE email = 'john.thompson@sq.example.com' LIMIT 1)
    ),

    (
        'Michael Brown Seaman Book',
        'Michael Brown',
        (SELECT user_id FROM users WHERE email = 'jennifer.white@sq.example.com' LIMIT 1)
    ),

    (
        'Michael Brown Employment Agreement',
        'Michael Brown',
        (SELECT user_id FROM users WHERE email = 'manager@sq.example.com' LIMIT 1)
    ),

    (
        'Michael Brown Vaccination Record',
        'Michael Brown',
        (SELECT user_id FROM users WHERE email = 'eddy.ng@sq.example.com' LIMIT 1)
    ),

    (
        'Linda Martinez Passport',
        'Linda Martinez',
        (SELECT user_id FROM users WHERE email = 'andrew.lauwira@sq.example.com' LIMIT 1)
    ),

    (
        'Linda Martinez Visa',
        'Linda Martinez',
        (SELECT user_id FROM users WHERE email = 'maria.santos@sq.example.com' LIMIT 1)
    ),

    (
        'Linda Martinez Food Handling Certificate',
        'Linda Martinez',
        (SELECT user_id FROM users WHERE email = 'david.chen@sq.example.com' LIMIT 1)
    ),

    (
        'Linda Martinez Medical Certificate',
        'Linda Martinez',
        (SELECT user_id FROM users WHERE email = 'sarah.johnson@sq.example.com' LIMIT 1)
    ),

    (
        'David Wilson Passport',
        'David Wilson',
        (SELECT user_id FROM users WHERE email = 'michael.lee@sq.example.com' LIMIT 1)
    ),

    (
        'David Wilson Engineering Certificate',
        'David Wilson',
        (SELECT user_id FROM users WHERE email = 'linda.garcia@sq.example.com' LIMIT 1)
    ),

    (
        'David Wilson Employment Agreement',
        'David Wilson',
        (SELECT user_id FROM users WHERE email = 'admin@sq.example.com' LIMIT 1)
    ),

    (
        'David Wilson Safety Training Certificate',
        'David Wilson',
        (SELECT user_id FROM users WHERE email = 'robert.wilson@sq.example.com' LIMIT 1)
    ),

    (
        'Sarah Lee Passport',
        'Sarah Lee',
        (SELECT user_id FROM users WHERE email = 'james.anderson@sq.example.com' LIMIT 1)
    ),

    (
        'Sarah Lee Visa',
        'Sarah Lee',
        (SELECT user_id FROM users WHERE email = 'patricia.martinez@sq.example.com' LIMIT 1)
    ),

    (
        'Sarah Lee Vaccination Record',
        'Sarah Lee',
        (SELECT user_id FROM users WHERE email = 'eddy.ng@sq.example.com' LIMIT 1)
    ),

    (
        'Sarah Lee Crew Contract',
        'Sarah Lee',
        (SELECT user_id FROM users WHERE email = 'manager@sq.example.com' LIMIT 1)
    ),

    (
        'Andrew Davis Passport',
        'Andrew Davis',
        (SELECT user_id FROM users WHERE email = 'andrew.lauwira@sq.example.com' LIMIT 1)
    ),

    (
        'Andrew Davis Medical Certificate',
        'Andrew Davis',
        (SELECT user_id FROM users WHERE email = 'maria.santos@sq.example.com' LIMIT 1)
    ),

    (
        'Andrew Davis STCW Certificate',
        'Andrew Davis',
        (SELECT user_id FROM users WHERE email = 'compliance.admin@sq.example.com' LIMIT 1)
    ),

    (
        'Andrew Davis Employment Agreement',
        'Andrew Davis',
        (SELECT user_id FROM users WHERE email = 'admin@sq.example.com' LIMIT 1)
    ),

    (
        'Jessica Miller Passport',
        'Jessica Miller',
        (SELECT user_id FROM users WHERE email = 'david.chen@sq.example.com' LIMIT 1)
    ),

    (
        'Jessica Miller Visa',
        'Jessica Miller',
        (SELECT user_id FROM users WHERE email = 'sarah.johnson@sq.example.com' LIMIT 1)
    ),

    (
        'Jessica Miller Vaccination Record',
        'Jessica Miller',
        (SELECT user_id FROM users WHERE email = 'eddy.ng@sq.example.com' LIMIT 1)
    ),

    (
        'Jessica Miller Safety Training Certificate',
        'Jessica Miller',
        (SELECT user_id FROM users WHERE email = 'andrew.lauwira@sq.example.com' LIMIT 1)
    );
