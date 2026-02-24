# Human Body Simulator

A distributed .NET simulation of human physiology. Each organ or system runs as its own service, communicating via message queues and shared cache to model digestion, respiration, and circulation.

## Overview

The simulator models:

- **Digestive system** — Mouth → Stomach → Small Intestine: ingests food (CSV), chunks it, and processes nutrients (glucose, protein, fat) with ATP-driven motion.
- **Respiratory system** — Lungs: air diffusion (oxygen in, carbon dioxide out) across alveolar cells.
- **Circulatory system** — Left Atrium, Right Atrium: heart chambers that participate in blood flow and cell respiration (e.g. myocytes consuming oxygen and glucose).

Shared logic handles **blood** (oxygen, glucose, amino acids, fatty acids, water, CO₂), **cell respiration**, **blood diffusion**, and **messaging** between services. The **API** exposes simulation state (e.g. cached cell data in Redis) over HTTP.

## Architecture

- **Organ services**: Console apps (worker services) that run digestion, diffusion, or respiration jobs on a schedule (Quartz).
- **SharedLogic**: Library used by all services — digestion pipeline, respiration processors, blood/cell caching, Redis and RabbitMQ integration.
- **API**: ASP.NET Core 9 Web API with OpenAPI, backed by Redis for cache reads.
- **Infrastructure**: Redis (cache), RabbitMQ (messaging). All services and infra are wired in Docker Compose.

## Solution structure

| Project        | Description |
|----------------|-------------|
| **API**        | Web API; exposes Redis cache (e.g. `GET /api/cell/{key}`). 
| **Mouth**      | Digestive service; reads food, chunks it, passes to Stomach. 
| **Stomach**    | Digestive service; processes chunks, passes to Small Intestine. 
| **SmallIntestine** | Digestive service; nutrient diffusion, outputs toward Large Intestine.
| **Lungs**      | Respiratory service; air diffusion (O₂/CO₂) for alveolar cells.
| **LeftAtrium** | Heart chamber service; blood/cell processing. 
| **RightAtrium**| Heart chamber service; blood/cell processing.
| **SharedLogic**| Shared models, digestion/respiration/diffusion logic, Redis/RabbitMQ/Quartz. 
| **Test** / **BiologyTests** | Test projects. 

## Technology stack

- **.NET 9** — All projects target `net9.0`.
- **Redis** (StackExchange.Redis) — Caching of blood/cell state; API reads from Redis.
- **RabbitMQ** — Messaging between organ services.
- **Quartz** — Scheduled jobs (digestion, respiration, blood diffusion).
- **ASP.NET Core** — API with OpenAPI support.

## License

See repository license file (if present).
