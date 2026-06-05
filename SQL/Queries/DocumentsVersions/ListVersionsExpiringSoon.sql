SELECT dv.version_id, dv.version_number, dv.expiration_date, d.document_id, d.title, d.crew_name
FROM documents_versions dv
INNER JOIN documents d ON dv.document_id = d.document_id
WHERE dv.expiration_date >= NOW()
    AND dv.expiration_date <= DATE_ADD(NOW(), INTERVAL @Months MONTH)
ORDER BY dv.expiration_date;