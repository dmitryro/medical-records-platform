-- Creating roles table
CREATE TABLE public.roles (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL,
    permissions TEXT[],
    created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Creating users table
CREATE TABLE public.users (
    id SERIAL PRIMARY KEY,
    first TEXT NOT NULL,
    last TEXT NOT NULL,
    username TEXT NOT NULL,
    email TEXT NOT NULL,
    password TEXT NOT NULL,
    phone TEXT,
    created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    role_id INTEGER NOT NULL,
    FOREIGN KEY (role_id) REFERENCES public.roles(id)
);

-- Creating logins table
CREATE TABLE public.logins (
    id SERIAL PRIMARY KEY,
    username TEXT NOT NULL,
    password TEXT NOT NULL,
    login_time TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Creating registrations table
CREATE TABLE public.registrations (
    id SERIAL PRIMARY KEY,
    first TEXT NOT NULL,
    last TEXT NOT NULL,
    username TEXT NOT NULL,
    email TEXT NOT NULL,
    password TEXT NOT NULL,
    phone TEXT,
    role_id INTEGER NOT NULL,
    created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (role_id) REFERENCES public.roles(id)
);

-- Creating master_patient_index table
CREATE TABLE public.master_patient_index (
    id SERIAL PRIMARY KEY,
    patient_id INTEGER UNIQUE NOT NULL,
    first_name TEXT NOT NULL,
    last_name TEXT NOT NULL,
    date_of_birth DATE NOT NULL,
    gender TEXT NOT NULL,
    address TEXT,
    contact_number TEXT,
    email TEXT,
    social_security_number TEXT,
    match_score REAL,
    match_date DATE,
    created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- Creating patients table
CREATE TABLE public.patients (
    id SERIAL PRIMARY KEY,
    first_name TEXT,
    last_name TEXT,
    date_of_birth DATE,
    gender TEXT,
    address TEXT,
    contact_number TEXT,
    email TEXT,
    social_security_number TEXT,
    mpi_id INTEGER,
    FOREIGN KEY (mpi_id) REFERENCES public.master_patient_index(id)
);

-- Creating vitals table
CREATE TABLE public.vitals (
    id SERIAL PRIMARY KEY,
    patient_id INTEGER NOT NULL,
    weight REAL,
    height REAL,
    blood_pressure_systolic INTEGER,
    blood_pressure_diastolic INTEGER,
    temperature REAL,
    heart_rate INTEGER,
    created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (patient_id) REFERENCES public.patients(id)
);

-- Creating events table

CREATE TABLE public.events (
    id SERIAL PRIMARY KEY,
    patient_id INTEGER NOT NULL,
    event_type TEXT NOT NULL,
    event_date DATE NOT NULL,
    description TEXT NOT NULL,
    CONSTRAINT fk_patient
        FOREIGN KEY(patient_id)
            REFERENCES public.patients(id)
            ON DELETE CASCADE
);

