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

-- Creating billing_addresses table
CREATE TABLE public.billing_addresses (
    id SERIAL PRIMARY KEY,
    patient_id INTEGER REFERENCES public.master_patient_index(id),
    address TEXT,
    city TEXT,
    state TEXT,
    postal_code TEXT,
    country TEXT
);

-- Creating doctors table
CREATE TABLE public.doctors (
    id SERIAL PRIMARY KEY,
    first_name TEXT NOT NULL,
    last_name TEXT NOT NULL,
    phone TEXT NOT NULL,
    email TEXT NOT NULL,
    specialization TEXT NOT NULL,
    license_number TEXT NOT NULL
);


-- Creating insurances table
CREATE TABLE public.insurances (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL,
    contact_info TEXT NOT NULL,
    coverage_details TEXT,
    claims_integration_status TEXT NOT NULL
);

-- Creating claims table
CREATE TABLE public.claims (
    id SERIAL PRIMARY KEY,
    patient_id INTEGER NOT NULL REFERENCES public.patients(id) ON DELETE CASCADE,
    insurance_id INTEGER NOT NULL REFERENCES public.insurances(id) ON DELETE CASCADE,
    date_of_service DATE NOT NULL,
    amount_billed REAL NOT NULL,
    amount_covered REAL NOT NULL,
    status TEXT NOT NULL
);

-- Creating clinical_notes table
CREATE TABLE public.clinical_notes (
    id SERIAL PRIMARY KEY,
    patient_id INTEGER NOT NULL REFERENCES public.patients(id) ON DELETE CASCADE,
    doctor_id INTEGER NOT NULL REFERENCES public.doctors(id) ON DELETE CASCADE,
    note_text TEXT NOT NULL,
    created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT now(),
    updated_at TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT now()
);

-- Create medical_codes table
CREATE TABLE public.medical_codes (
    id SERIAL PRIMARY KEY,
    code TEXT NOT NULL,
    description TEXT NOT NULL,
    code_type TEXT NOT NULL
);

-- Create diagnoses table
CREATE TABLE public.diagnoses (
    id SERIAL PRIMARY KEY,
    patient_id INTEGER NOT NULL,
    doctor_id INTEGER NOT NULL,
    code_id INTEGER NOT NULL,
    description TEXT NOT NULL,
    date DATE NOT NULL,
    CONSTRAINT diagnoses_patient_id_fkey FOREIGN KEY (patient_id) REFERENCES public.patients(id),
    CONSTRAINT diagnoses_doctor_id_fkey FOREIGN KEY (doctor_id) REFERENCES public.doctors(id),
    CONSTRAINT diagnoses_code_id_fkey FOREIGN KEY (code_id) REFERENCES public.medical_codes(id)
);

-- Create medications table
CREATE TABLE public.medications (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL,
    brand_name TEXT,
    generic_name TEXT,
    medication_class TEXT NOT NULL
);

-- Create dosages tabe
CREATE TABLE public.dosages (
    id SERIAL PRIMARY KEY,
    medication_id INT NOT NULL,
    dosage_amount TEXT NOT NULL,
    dosage_frequency TEXT NOT NULL,
    created_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP,
    FOREIGN KEY (medication_id) REFERENCES public.medications(id)
);

-- Create encounters table
CREATE TABLE public.encounters (
    id SERIAL PRIMARY KEY,
    patient_id INT NOT NULL,
    doctor_id INT NOT NULL,
    encounter_type TEXT NOT NULL,
    date DATE NOT NULL,
    notes TEXT,
    FOREIGN KEY (patient_id) REFERENCES public.patients(id),
    FOREIGN KEY (doctor_id) REFERENCES public.doctors(id)
);

-- Create fhir_messages table
CREATE TABLE public.fhir_messages (
    id SERIAL PRIMARY KEY,
    message_type TEXT NOT NULL,
    message_content TEXT NOT NULL,
    received_date TIMESTAMP NOT NULL,
    sent_date TIMESTAMP,
    status TEXT NOT NULL
);

-- Create hl7_messages table
CREATE TABLE public.hl7_messages (
    id SERIAL PRIMARY KEY,
    message_type TEXT NOT NULL,
    message_content TEXT NOT NULL,
    received_date TIMESTAMP NOT NULL,
    sent_date TIMESTAMP,
    status TEXT NOT NULL
);

-- Create x12_edi_messages table
CREATE TABLE public.x12_edi_messages (
    id SERIAL PRIMARY KEY,
    transaction_set_id TEXT NOT NULL,
    transaction_set_control_number TEXT NOT NULL,
    interchange_control_number TEXT NOT NULL,
    sender_id TEXT NOT NULL,
    receiver_id TEXT NOT NULL,
    message_content TEXT NOT NULL,
    received_date TIMESTAMP NOT NULL,
    sent_date TIMESTAMP,
    status TEXT NOT NULL
);

-- Create immunizations table
CREATE TABLE public.immunizations (
    id SERIAL PRIMARY KEY,
    patient_id INTEGER NOT NULL,
    vaccine_name TEXT NOT NULL,
    administration_date DATE NOT NULL,
    administered_by INTEGER,
    notes TEXT,
    FOREIGN KEY (patient_id) REFERENCES public.patients(id),
    FOREIGN KEY (administered_by) REFERENCES public.users(id)
);

-- Create medical_records table
CREATE TABLE public.medical_records (
    id SERIAL PRIMARY KEY,
    patient_id INTEGER NOT NULL,
    doctor_id INTEGER NOT NULL,
    record_type TEXT,
    record_data JSONB,
    created_at TIMESTAMPTZ NOT NULL,
    updated_at TIMESTAMPTZ NOT NULL,
    FOREIGN KEY (patient_id) REFERENCES public.patients(id),
    FOREIGN KEY (doctor_id) REFERENCES public.doctors(id)
);

-- Create medication_interactions table
CREATE TABLE public.medication_interactions (
    id SERIAL PRIMARY KEY,
    primary_medication_id INTEGER NOT NULL,
    secondary_medication_id INTEGER NOT NULL,
    FOREIGN KEY (primary_medication_id) REFERENCES public.medications(id) ON DELETE CASCADE,
    FOREIGN KEY (secondary_medication_id) REFERENCES public.medications(id) ON DELETE CASCADE
);

-- Create medication_interaction_primaries table
CREATE TABLE public.medication_interaction_primaries (
    id SERIAL PRIMARY KEY,
    medication_id INTEGER NOT NULL,
    interaction_name VARCHAR(255) NOT NULL,
    interaction_class VARCHAR(255) NOT NULL,
    description TEXT,
    FOREIGN KEY (medication_id) REFERENCES public.medications(id) ON DELETE CASCADE
);

-- Create medication_interaction_secondaries table
CREATE TABLE public.medication_interaction_secondaries (
    id SERIAL PRIMARY KEY,
    primary_medication_id INTEGER NOT NULL,
    secondary_medication_id INTEGER NOT NULL,
    severity VARCHAR(255) NOT NULL,
    description TEXT,
    FOREIGN KEY (primary_medication_id) REFERENCES public.medication_interaction_primaries(id) ON DELETE CASCADE,
    FOREIGN KEY (secondary_medication_id) REFERENCES public.medications(id) ON DELETE CASCADE
);


-- Create nurses table
CREATE TABLE public.nurses (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    credentials VARCHAR(255) NOT NULL,
    specialization VARCHAR(255),
    assigned_doctor_id INTEGER,
    shift_schedule TEXT,
    contact_info VARCHAR(255) NOT NULL,
    employment_status VARCHAR(255) NOT NULL,
    FOREIGN KEY (assigned_doctor_id) REFERENCES public.doctors(id) ON DELETE SET NULL
);

-- Create partners table
CREATE TABLE public.partners (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    contact_info VARCHAR(255) NOT NULL,
    partnership_type VARCHAR(255) NOT NULL
);

-- Create prescriptions table
CREATE TABLE public.prescriptions (
    id SERIAL PRIMARY KEY,
    patient_id INT NOT NULL,
    doctor_id INT NOT NULL,
    medication_name VARCHAR(255) NOT NULL,
    dose VARCHAR(255) NOT NULL,
    frequency VARCHAR(255) NOT NULL,
    start_date TIMESTAMP NOT NULL,
    end_date TIMESTAMP,
    FOREIGN KEY (patient_id) REFERENCES public.patients(id),
    FOREIGN KEY (doctor_id) REFERENCES public.doctors(id)
);

-- Create patient_journeys table
CREATE TABLE public.patient_journeys (
    id SERIAL PRIMARY KEY,
    patient_id INT NOT NULL,
    encounter_id INT NOT NULL,
    diagnosis_id INT NOT NULL,
    prescription_id INT,
    vitals_id INT,
    timestamp TIMESTAMP NOT NULL,
    FOREIGN KEY (patient_id) REFERENCES public.patients(id),
    FOREIGN KEY (encounter_id) REFERENCES public.encounters(id),
    FOREIGN KEY (diagnosis_id) REFERENCES public.diagnoses(id),
    FOREIGN KEY (prescription_id) REFERENCES public.prescriptions(id),
    FOREIGN KEY (vitals_id) REFERENCES public.vitals(id)
);

-- Create pharmacies table
CREATE TABLE public.pharmacies (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    address VARCHAR(255),
    contact_number VARCHAR(20),
    email VARCHAR(255),
    pharmacy_type VARCHAR(100)
);

-- Create pharmacy_integrations table
CREATE TABLE public.pharmacy_integrations (
    id SERIAL PRIMARY KEY,
    pharmacy_id INTEGER NOT NULL,
    prescription_id INTEGER NOT NULL,
    status VARCHAR(255) NOT NULL,
    fulfillment_date TIMESTAMP,

    CONSTRAINT fk_pharmacy
        FOREIGN KEY (pharmacy_id)
        REFERENCES public.pharmacies (id)
        ON DELETE CASCADE,

    CONSTRAINT fk_prescription
        FOREIGN KEY (prescription_id)
        REFERENCES public.prescriptions (id)
        ON DELETE CASCADE
);

-- Create refills table
CREATE TABLE public.refills (
    id SERIAL PRIMARY KEY,
    prescription_id INTEGER NOT NULL,
    date_requested TIMESTAMP NOT NULL,
    date_fulfilled TIMESTAMP,
    status VARCHAR(255) NOT NULL,
    CONSTRAINT fk_prescription
        FOREIGN KEY (prescription_id)
        REFERENCES public.prescriptions (id)
        ON DELETE CASCADE
);

-- Create side_effects table
CREATE TABLE public.side_effects (
    id SERIAL PRIMARY KEY,
    medication_id INTEGER NOT NULL,
    description TEXT NOT NULL,
    severity VARCHAR(50) NOT NULL,
    onset TEXT,
    duration TEXT,
    created_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP NOT NULL,
    CONSTRAINT fk_medication
        FOREIGN KEY (medication_id)
        REFERENCES public.medications (id)
        ON DELETE CASCADE
);

-- Create social_determinants table
CREATE TABLE public.social_determinants (
    id SERIAL PRIMARY KEY,
    patient_id INTEGER NOT NULL,
    factor_type TEXT NOT NULL,
    details TEXT,
    recorded_by INTEGER,
    recorded_at TIMESTAMP NOT NULL,
    CONSTRAINT fk_social_determinants_patient
        FOREIGN KEY (patient_id)
        REFERENCES public.patients (id)
        ON DELETE CASCADE,
    CONSTRAINT fk_social_determinants_user
        FOREIGN KEY (recorded_by)
        REFERENCES public.users (id)
        ON DELETE SET NULL
);
