# PortStream OS - Comprehensive Technical Documentation

## 1. Project Overview
**PortStream OS** is an enterprise-grade, distributed Port Management System designed to handle the complex logistical, regulatory, and financial operations of a modern maritime port. It transitions away from monolithic legacy architectures by utilizing an event-driven microservices ecosystem.

## 2. Technical Stack
* **Backend Framework:** .NET 10 Web API & Worker Services
* **Frontend Framework:** Next.js (App Router), React, Tailwind CSS v4
* **API Gateway:** Ocelot
* **Relational Databases:** PostgreSQL (Entity Framework Core)
* **In-Memory Caching:** Redis (StackExchange.Redis)
* **Event Streaming / Message Broker:** Apache Kafka (Confluent.Kafka)
* **Logging:** Serilog
* **Email Automation:** MailKit (SMTP)
* **Containerization:** Docker & Docker Compose

---

## 3. Microservices Architecture & Functionality

The backend is composed of **6 independent microservices** running behind a centralized API Gateway.

### A. Ocelot API Gateway (Port 5000)
* **Functionality:** Acts as the single entry point for the Next.js frontend. It intercepts all incoming HTTP requests (e.g., `/api/vessels`, `/api/gate`) and dynamically routes them to the correct underlying downstream microservice port. 

### B. Vessel Traffic Service (Port 5001)
* **Functionality:** Manages the maritime side of the port.
* **Core Entities:** `Vessel`, `BerthAllocation`.
* **Operations:** 
    * Tracks incoming and outgoing ships (Active Channel Queue).
    * Handles Berth Allocation, assigning physical docking locations and time slots to incoming vessels to prevent collisions and optimize dock usage.

### C. Yard & Equipment Service (Port 5002)
* **Functionality:** Manages the physical storage of containers on land and the heavy equipment used to move them.
* **Core Entities:** `YardBin`, `Container`.
* **Key Tech (Redis):** Because yard telemetry updates thousands of times per minute, this service utilizes a `YardCacheService`. It caches the heavy "Yard Heatmap" queries in Redis memory for 60 seconds, drastically reducing the load on the PostgreSQL database and enabling sub-millisecond frontend polling.

### D. Customs & Risk Service (Port 5003)
* **Functionality:** A business-logic engine responsible for regulatory compliance and contraband prevention.
* **Core Entities:** `CustomsManifest`, `CargoItem`, `RiskRule`.
* **Operations:** 
    * Administrators can dynamically create `RiskRules` (e.g., "Flag if Weight > 50,000kg").
    * The `RiskAssessmentService` automatically audits every incoming manifest against these rules, calculating a final `RiskScore` (Low, Medium, Critical). Critical manifests are flagged for physical inspection.

### E. Gate & Billing Service (Port 5004)
* **Functionality:** Handles terrestrial truck traffic and automated tariff generation via event-driven pub/sub.
* **Core Entities:** `GateEvent`, `Invoice`.
* **Operations (Kafka Producer):** Provides endpoints for trucks entering/exiting the port. Upon an exit event, it acts as a Kafka Producer, publishing a `truck-gate-events` message.
* **Operations (Kafka Consumer):** Contains a background worker that listens to its own Kafka topics. When it catches a truck exit, it asynchronously calculates the port tariff and generates an `Invoice` directly into the database without blocking the API thread.

### F. Operations Service (Port 5005)
* **Functionality:** Manages Green Port environmental compliance and Stevedore (labor) safety.
* **Core Entities:** `EnvironmentReading`, `WorkerShift`.
* **Operations:** 
    * Tracks live sensor data across the port (Air Quality Index, CO2 Emissions, Noise Levels), automatically flagging non-compliant zones.
    * Manages workforce schedules, enforcing hard software locks if a worker's shift exceeds the legal 12-hour limit or if they lack verified safety gear.

### G. Notification Service (Background Worker)
* **Functionality:** A headless background service responsible for asynchronous alerts.
* **Key Tech (Kafka + SMTP):** It does not expose any REST API. Instead, it subscribes to the `truck-gate-events` Kafka topic. When a truck exits the port (published by the Gate service), this service instantly catches the event and utilizes `MailKit` to dispatch an automated "Gate Exit & Invoice Receipt" email via SMTP.

---

## 4. Frontend UI (Next.js)
The frontend consumes the API Gateway and provides a modular Dashboard:
1. **Vessel Traffic Kanban:** Visualizes berth allocations.
2. **Yard Heatmap:** A visual color-coded grid representing bin capacity.
3. **Customs Desk:** A dynamic rule builder and risk-flagged manifest table.
4. **Gate & Billing Log:** Live terrestrial traffic logs and automated invoice displays using glassmorphism UI.
5. **Environmental Dashboard:** Live, auto-updating charts and alerts for Green Port compliance and worker safety tracking.
