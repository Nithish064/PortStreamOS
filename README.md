# PortStream OS - Enterprise Port Management System

PortStream OS is an enterprise-grade, distributed maritime and logistics management platform engineered to handle complex vessel traffic, yard telemetry, customs compliance, terrestrial gate operations, automated billing tariffs, environmental monitoring, and workforce safety.

---

## 🏛️ System Architecture

```
                    +---------------------------+
                    |    Next.js 16+ Frontend   |
                    | (Tailwind CSS, Dashboard) |
                    +-------------+-------------+
                                  |
                                  v
                    +---------------------------+
                    |    Ocelot API Gateway     |
                    |       (Port 5000)         |
                    +-------------+-------------+
                                  |
    +---------------+-------------+-------------+---------------+---------------+
    |               |                           |               |               |
    v               v                           v               v               v
+-------+     +-----------+               +-----------+   +-----------+   +-----------+
| Vessel|     |   Yard    |               |  Customs  |   |   Gate    |   |Operations |
|Traffic|     | Equipment |               |   Risk    |   |  Billing  |   |Environment|
|(5001) |     |  (5002)   |               |  (5003)   |   |  (5004)   |   |  (5005)   |
+-------+     +-----+-----+               +-----------+   +-----+-----+   +-----------+
                    |                                           |
             [Redis Cache]                          [Apache Kafka Broker]
                                                                |
                                                                v
                                                      +-------------------+
                                                      |   Notification    |
                                                      |   (Worker/SMTP)   |
                                                      +-------------------+
```

---

## 🚀 Core Microservices & Functionality

1. **Ocelot API Gateway (`Port 5000`)**
   * Acts as the unified entry point for frontend clients.
   * Centralizes routing, request forwarding, and service abstraction.

2. **Vessel Traffic Service (`Port 5001`)**
   * Manages maritime queues, active channel traffic, and physical berth/dock allocations.
   * Prevents harbor collisions and optimizes dock turnaround time.

3. **Yard & Equipment Service (`Port 5002`)**
   * Tracks container storage across terminal yards and manages heavy machinery.
   * Implements an in-memory **Redis** caching layer to serve high-frequency yard heatmap telemetry with sub-millisecond latency, shielding PostgreSQL from query spikes.

4. **Customs & Risk Service (`Port 5003`)**
   * Regulatory compliance engine that validates incoming cargo manifests.
   * Uses dynamic rule sets (e.g. weight, origin, hazardous flags) to calculate composite risk scores and flag contraband shipments for physical inspection.

5. **Gate & Billing Service (`Port 5004`)**
   * Tracks terrestrial truck ingresses and egresses.
   * Functions as an **Apache Kafka Producer** on gate departure events (`truck-gate-events`) and triggers asynchronous automated tariff invoicing.

6. **Operations & Workforce Service (`Port 5005`)**
   * Monitors real-time environmental sensors (AQI, CO2 emissions, noise levels) for Green Port compliance.
   * Manages stevedore shifts, automatically enforcing statutory rest limits and safety compliance.

7. **Notification Service (Background Worker)**
   * Headless background consumer that subscribes to Kafka topics (`truck-gate-events`).
   * Dispatches automated gate exit receipts and invoice notifications via **MailKit (SMTP)**.

---

## 💻 Tech Stack

* **Backend:** .NET 10 / C#, Entity Framework Core, Ocelot API Gateway, Serilog
* **Frontend:** Next.js 16 (App Router), React, Tailwind CSS
* **Databases & Caching:** PostgreSQL, Redis (StackExchange.Redis)
* **Message Broker:** Apache Kafka (Confluent.Kafka)
* **Email & Alerts:** MailKit (SMTP)
* **Containerization:** Docker & Docker Compose

---

## 🛠️ Getting Started

### Prerequisites
* [.NET 10 SDK](https://dotnet.microsoft.com/)
* [Node.js 20+](https://nodejs.org/)
* [Docker Desktop](https://www.docker.com/)

### 1. Start Infrastructure Dependencies
```bash
docker compose up -d
```
Starts PostgreSQL, Redis, and Apache Kafka.

### 2. Run the Backend Solution
```bash
cd backend
dotnet build PortStream.slnx
# Start individual microservices or gateway via dotnet run
```

### 3. Run the Frontend Dashboard
```bash
cd frontend
npm install
npm run dev
```
Open [http://localhost:3000](http://localhost:3000) to view the PortStream OS control room.
