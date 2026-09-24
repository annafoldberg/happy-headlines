INSERT INTO profanities (term)
VALUES
    ('shit'),
    ('shitty')
    ('damn'),
    ('dammit')
ON CONFLICT DO NOTHING;