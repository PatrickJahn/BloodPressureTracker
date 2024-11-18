
# Blood Pressure Tracker

## Solution Overview

The **Blood Pressure Tracker** is a distributed microservices-based application designed to monitor and track patient health data such as blood pressure measurements. The architecture follows a region-based model to cater to different geographical regions in Denmark. The services interact through an API Gateway, and observability is enhanced with logging and tracing tools.

### Projects in the Solution

1. **PatientService**:
    - Handles operations related to patient data (CRUD operations).
    - Utilizes a region-specific database for patient data storage.
    - Implements feature flag checks for flexibility.

2. **MeasurementService**:
    - Manages blood pressure measurements for patients.
    - Supports operations like fetching measurements by SSN and adding new measurements.
    - Leverages region-specific databases and feature flags.

3. **Monitoring**:
    - Integrates **Seq** and **Zipkin** for logging and distributed tracing.
    - Provides observability for troubleshooting and performance analysis.

4. **APIGateway**:
    - Routes requests to respective services based on region.
    - Facilitates fault isolation and supports fallback mechanisms.
    - Exposes a unified API interface for clients.

---

## Getting Started

### Prerequisites
- Docker
- Docker Compose
- .NET 8 SDK
- Git

### Steps to Set Up Locally

1. Clone the repository:
   ```bash
   git clone https://github.com/tboulund-dls/BloodPressureTracker.git
   cd BloodPressureTracker
   ```

2. Build and run the services using Docker Compose:
   ```bash
   docker-compose up --build
   ```

3. Access the services:
    - **API Gateway**: [http://localhost:5001](http://localhost:5001)
    - **Seq** (Logs): [http://localhost:5341](http://localhost:5341)
    - **Zipkin** (Tracing): [http://localhost:9411](http://localhost:9411)

4. Explore the Swagger UI:
    - Patient Service APIs: `http://<region-service-url>/swagger`
    - Measurement Service APIs: `http://<region-service-url>/swagger`

---

## Sequence Diagrams

- In root there is a diagram folder that contains some sequence diagrams of different flows of the system. 
---

## Observability

- **Logging**: All services are configured to log events using Serilog and are centralized in **Seq**.
- **Tracing**: Distributed tracing is implemented with OpenTelemetry, and traces are sent to **Zipkin** for visualization.

---

## Key Features Implemented
- **Region-Specific Services**: Each region has dedicated databases and service instances.
- **Feature Flags**: Feature-based enablement for endpoints using a NuGet package.
- **CI/CD Pipeline**: Automates building, testing, and deployment of the services.
- **Distributed Observability**: Monitoring using Seq and Zipkin for logs and traces.

---
