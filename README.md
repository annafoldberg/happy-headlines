# Happy Headlines System

## Architecture

### C4 Diagrams
> Note: In the project description, it's specified that users can also share articles, however no part of the system indicates that anything related to that should be implemented. Therefore, I have left this out of the diagrams.

#### System Context Diagram
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/revised-context-diagram-dark.png">
  <img alt="Revised C4 system context diagram for Happy Headlines system" src="./docs/figures/revised-context-diagram-light.png" width="50%">
</picture>

<br><br>

#### Container Diagram
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/revised-container-diagram-dark.png">
  <img alt="Revised C4 container diagram for Happy Headlines system" src="./docs/figures/revised-container-diagram-light.png">
</picture>

### Article Management
Management and storage of published articles.

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/article-management-container-view-dark.png">
  <img alt="Container view" src="./docs/figures/article-management-container-view-light.png" width="180">
</picture>

##### Scaling
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/article-architecture-diagram-dark.png">
  <img alt="ArticleService and ArticleDatabase architecture diagram" src="./docs/figures/article-architecture-diagram-light.png">
</picture>
<br><br>

The `ArticleService` is horizontally scaled across three instances using Docker Swarm. Incoming requests are distributed between the available instances.  

The `ArticleDatabase` is split by continent, with one database for each continent and an additional global database for articles relevant worldwide.  

Each `ArticleService` instance can access all eight databases and routes database operations according to the continent specified by the request.

#### ArticleService
Manages published articles and subscribes to publication events.

##### API Endpoints
| Method | Endpoint | Parameters | Body | Description |
|---|---|---|---|---|
| `POST` | `/Articles/{continent}` | `continent` | `CreateArticleDto` | Create a published article in the specified continent database |
| `GET` | `/Articles/{continent}/{id}` | `continent`, `id` | — | Retrieve a published article |
| `PATCH` | `/Articles/{continent}/{id}` | `continent`, `id` | `UpdateArticleDto` | Update a published article |
| `DELETE` | `/Articles/{continent}/{id}` | `continent`, `id` | — | Delete a published article |

Valid `continent` values:
- `Africa`
- `Antarctica`
- `Asia`
- `Europe`
- `NorthAmerica`
- `Oceania`
- `SouthAmerica`
- `Global`

<br>

`CreateArticleDto` contains required fields:
```json
{
  "author": "string",
  "title": "string",
  "content": "string"
}
```

`UpdateArticleDto` contains optional fields:
```json
{
  "author": "string",
  "title": "string",
  "content": "string"
}
```

##### Internal Flows
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/article-service-flow-dark.png">
  <img alt="Article flow controller" src="./docs/figures/article-service-flow-light.png">
</picture>

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/article-service-flow-rabbitmq-dark.png">
  <img alt="Article flow message queue" src="./docs/figures/article-service-flow-rabbitmq-light.png">
</picture>

### ArticleDatabase
Stores published articles. Contains `articles` table.

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/article-database-dark.png">
  <img alt="Articles table" src="./docs/figures/article-database-light.png" width="300" >
</picture>

### Comment Management
Management, storage, and profanity filtering of comments.

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/comment-management-container-view-dark.png">
  <img alt="Container view" src="./docs/figures/comment-management-container-view-light.png" width="450">
</picture>

##### Fault Isolation between Services
Within this scope, fault isolation is supported by:
- Each service owns its own database.
- A circuit breaker limits the impact of ProfanityService failues on CommentService.
- Operations that don't require profanity filtering can continue independently.

However, the implementation does not fully satisfy strict swimlane principles, as successful comment creation and content updates depend on a response from ProfanityService.

#### ProfanityService
Filters profanity from content.

##### API Endpoints
| Method | Endpoint | Parameters | Body | Description |
|---|---|---|---|---|
| `POST` | `/Profanities/filter` | — | `FilterContentRequest` | Filter profanity from content |

<br>

`FilterContentRequest` contains a required field:
```json
{
  "content": "string"
}
```

`FilterContentResponse` contains an optional field:
```json
{
  "content": "string"
}
```

##### Internal Flow
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/profanity-service-flow-dark.png">
  <img alt="Profanity flow" src="./docs/figures/profanity-service-flow-light.png">
</picture>

#### ProfanityDatabase
Contains `profanities` table.

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/profanity-database-dark.png">
  <img alt="Profanities table" src="./docs/figures/profanity-database-light.png" width="220" >
</picture>

> Note: `term` also has a unique index on `lower(term)`, enforcing case-insensitive uniqueness.

#### CommentService
Manages comments.

##### API Endpoints
| Method | Endpoint | Parameters | Body | Description |
|---|---|---|---|---|
| `POST` | `/Comments/{articleId}` | `articleId` | `CreateCommentRequest` | Create a comment for an article |
| `GET` | `/Comments/article/{articleId}` | `articleId` | — | Retrieve all comments for an article |
| `GET` | `/Comments/{id}` | `id` | — | Retrieve a comment |
| `PATCH` | `/Comments/{id}` | `id` | `UpdateCommentRequest` | Update a comment |
| `DELETE` | `/Comments/{id}` | `id` | — | Delete a comment |

<br>

`CreateCommentRequest` contains required fields:
```json
{
  "author": "string",
  "content": "string"
}
```

`UpdateCommentRequest` contains optional fields:
```json
{
  "content": "string"
}
```

##### Internal Flow
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/comment-service-flow-dark.png">
  <img alt="Comment flow" src="./docs/figures/comment-service-flow-light.png">
</picture>

##### Circuit Breaker Pattern
- Implemented with Polly to handle failues when communicating with ProfanityService.
- Monitors requests over a 10-second period and opens when at least 3 requests have been made and 50% or more have failed
- Prevents further requests to ProfanitySrevice for 30 seconds while the circuit is open.
- Allows a test request after 30 seconds to determine whether ProfanityService has recovered.
- If ProfanityService remains unavailable, comment creation and content updates fail, while comment retrieval, deletion, and author-only updates remain available.

##### CommentDatabase
Stores profanity-filtered comments. Contains `comments` table.

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/comment-database-dark.png">
  <img alt="Comments table" src="./docs/figures/comment-database-light.png" width="300" >
</picture>

### Monitoring
Shared library that provides reusable logging and tracing functionality across services using Serilog and OpenTelemetry.  
Service names identify the source of traces.  

### Draft Management
Management and storage of drafts.

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/draft-management-container-view-dark.png">
  <img alt="Container view" src="./docs/figures/draft-management-container-view-light.png" width="200">
</picture>

##### Monitoring Plan
Focus on logging significant events and tracing application operations to support troubleshooting without generating unnecessary logs. Collection of logs and traces centrally to simplify troubleshooting.

<b>Logging:</b>
- Use information logs for successful operations that modify data to provide a record of changes.
- Use warning logs for unsuccessful update and delete operations to highlight potentially unexpected or problematic behaviour.
- Use debug logs for less significant events, such as retrieval of nonexisting drafts.
- Avoid logging method invocation as tracing will provide visibility into operation execution.

<b>Tracing:</b>
- Trace all CRUD operations to identify execution times and potential performance issues.
- Trace incoming HTTP requests and support tracing outgoing HTTP calls to provide visibility across service boundaries.

#### DraftService
Manages drafts.

##### API Endpoints
| Method | Endpoint | Parameters | Body | Description |
|---|---|---|---|---|
| `POST` | `/Drafts` | — | `DraftRequest` | Create a draft |
| `GET` | `/Drafts/{id}` | `id` | — | Retrieve a draft |
| `PUT` | `/Drafts/{id}` | `id` | `DraftRequest` | Update a draft |
| `DELETE` | `/Drafts/{id}` | `id` | — | Delete a draft |

<br>

`DraftRequest` contains optional fields:
```json
{
  "author": "string",
  "title": "string",
  "content": "string"
}
```

##### Internal Flow
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/draft-service-flow-dark.png">
  <img alt="Draft flow" src="./docs/figures/draft-service-flow-light.png">
</picture>

##### Monitoring Implementation
<b>Logging:</b>
- Structured logging using Serilog with logs collected centrally in Seq.
- Information logs for successful creation, update, and deletion of drafts.
- Warning logs for unsuccessful attempts to update or delete drafts that do not exist.
- Debug logs when attempting to retrieve a draft that does not exist.

<picture>
  <img alt="DraftService logs" src="./docs/figures/draft-logs.png">
</picture>

<b>Tracing:</b>
- Tracing using OpenTelemetry, with traces collected centrally in Aspire Dashboard.
- Traces the execution of DraftService's CRUD operations, including their duration.
- Automatic instrumentation captures incoming HTTP requests and supports tracing of outgoing HTTP calls.

<picture>
  <img alt="DraftService traces" src="./docs/figures/draft-traces.png">
</picture>

##### DraftDatabase
Stores drafts. Contains `drafts` table.

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/draft-database-dark.png">
  <img alt="Drafts table" src="./docs/figures/draft-database-light.png" width="300" >
</picture>

### Messaging
Shared library. Uses RabbitMQ to provide reusable functionality for asynchronous communication between services, including event publishing, consumption, and distributed tracing.

### Publication
Publication, distribution, storage, and newsletter retrieval of published articles.

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/publication-container-view-dark.png">
  <img alt="Container view" src="./docs/figures/publication-container-view-light.png">
</picture>

##### Distributed Tracing Plan
- Trace significant application operations related to publication and newsletter retrieval to identify execution times and potential performance issues in this scope.
- Trace incoming HTTP requests and outgoing HTTP calls to provide visibility across service boundaries.
- Trace RabbitMQ message publishing and consumption to track asynchronous communication between services.
- Propagate trace context across HTTP and RabbitMQ communication to maintain connected distributed traces.

##### Distributed Tracing Implementation
- Tracing using OpenTelemetry with traces collected centrally in Aspire Dashboard.
- Traces significant application operations related to publication and newsletter retrieval in PublisherService, ArticleService, and NewsletterService, including their duration.
- Automatic instrumentation captures incoming HTTP requests and outgoing HTTP calls.
- Traces RabbitMQ message publishing and consumption using producer and consumer spans.
- Trace context propagation through RabbitMQ message headers maintains connected traces across asynchronous service boundaries.

<picture>
  <img alt="Publication trace" src="./docs/figures/publication-trace.png">
</picture>

<br>

<picture>
  <img alt="Daily newsletter trace" src="./docs/figures/daily-newsletter-trace.png">
</picture>

<br>

<picture>
  <img alt="Traces dashboard" src="./docs/figures/traces-dashboard.png">
</picture>

#### PublisherService
Publishes articles to queue.

##### API Endpoints
| Method | Endpoint | Parameters | Body | Description |
|---|---|---|---|---|
| `POST` | `/Publications` | — | `CreatePublicationRequest` | Publish an article |

<br>

`CreatePublicationRequest` contains required fields:
```json
{
  "author": "string",
  "title": "string",
  "content": "string",
  "continent": "string"
}
```

Valid `continent` values:
- `Africa`
- `Antarctica`
- `Asia`
- `Europe`
- `NorthAmerica`
- `Oceania`
- `SouthAmerica`
- `Global`

##### Internal Flow
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/publisher-service-flow-dark.png">
  <img alt="Publisher flow" src="./docs/figures/publisher-service-flow-light.png">
</picture>

#### NewsletterService
Subscribes to published articles and retrieves article for daily newsletter.

##### API Endpoints
| Method | Endpoint | Parameters | Body | Description |
|---|---|---|---|---|
| `GET` | `/Newsletters/{continent}/daily` | `continent`, `date` | — | Retrieve the daily newsletter for the specified continent and date |

Valid `continent` values:
- `Africa`
- `Antarctica`
- `Asia`
- `Europe`
- `NorthAmerica`
- `Oceania`
- `SouthAmerica`
- `Global`

The `date` query parameter uses the format `YYYY-MM-DD`.

##### Internal Flows
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/newsletter-service-flow-dark.png">
  <img alt="Newsletter flow controller" src="./docs/figures/newsletter-service-flow-light.png">
</picture>

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/newsletter-service-flow-rabbitmq-dark.png">
  <img alt="Newsletter flow message queue" src="./docs/figures/newsletter-service-flow-rabbitmq-light.png">
</picture>

#### ArticleQueue
Handles asynchronous communication between PublisherService, ArticleService, and NewsletterService using RabbitMQ.

PublisherService publishes `ArticlePublished` events to the `article.published` fanout exchange, which distributes the events to separate queues for ArticleService and NewsletterService.

| Exchange | Type | Consumer | Queue |
|---|---|---|---|
| `article.published` | Fanout | ArticleService | `article.article-published` |
| `article.published` | Fanout | NewsletterService | `newsletter.article-published` |

The `ArticlePublished` event contains:
```json
{
  "articleId": "guid",
  "author": "string",
  "title": "string",
  "content": "string",
  "publicationTimestampUtc": "datetime",
  "continent": "string"
}
```

### Caching
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/cache-container-view-dark.png">
  <img alt="Container view" src="./docs/figures/cache-container-view-light.png">
</picture>
<br><br><br>

Prometheus is used to collect and store cache metrics. Grafana queries Prometheus to visualize the metrics in a dashboard.<br><br>
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/cache-metrics-dark.png">
  <img alt="Cache metrics" src="./docs/figures/cache-metrics-light.png">
</picture>
<br><br>

A Grafana dashboard displays cache hits, misses, and hit ratios for both caches, including a comparison of their hit ratios over time.<br><br>
<picture>
  <img alt="Grafana dashboard" src="./docs/figures/grafana-dashboard.png">
</picture>

#### ArticleCache
Periodic offline caching. The cache stores global articles published within the last 14 days. Articles older than 14 days are removed from the cache.

##### High-Level Overview of Cache Refresh
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/article-cache-refresh-dark.png">
  <img alt="Article cache refresh" src="./docs/figures/article-cache-refresh-light.png" width="220">
</picture>

##### High-Level Overview of Cache Retrieval
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/article-cache-retrieval-dark.png">
  <img alt="Article cache retrieval" src="./docs/figures/article-cache-retrieval-light.png" width="220">
</picture>

##### Offline Caching
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/articles-offline-caching-dark.png">
  <img alt="Offline caching of articles" src="./docs/figures/articles-offline-caching-light.png">
</picture>

##### Retrieval of Articles
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/article-cache-retrieval-low-level-dark.png">
  <img alt="Article cache retrieval" src="./docs/figures/article-cache-retrieval-low-level-light.png">
</picture>

#### CommentCache
Cache-on-miss approach. The cache stores comments for up to 30 recently accessed articles. An LRU (Least Recently Used) algorithm is used to evict the least recently accessed entries when the cache reaches its capacity.

##### High-Level Overview of Cache Retrieval, Population, and Eviction
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/comment-cache-retrieval-dark.png">
  <img alt="Comment cache retrieval, population, and eviction" src="./docs/figures/comment-cache-retrieval-light.png" width="500">
</picture>

##### High-Level Overview of Cache Invalidation
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/comment-cache-invalidation-dark.png">
  <img alt="Comment cache invalidation" src="./docs/figures/comment-cache-invalidation-light.png" width="220">
</picture>

##### Retrieval of Comments: Cache Hit
<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/comment-cache-hit-dark.png">
  <img alt="Comment cache hit" src="./docs/figures/comment-cache-hit-light.png" width="500">
</picture>

##### Retrieval of Comments: Cache Miss
> Note: Step 1 of this diagram takes place after the first three steps of the Cache Hit diagram.

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/comment-cache-miss-dark.png">
  <img alt="Comment cache miss" src="./docs/figures/comment-cache-miss-light.png">
</picture>

##### Cache Invalidation
> Note: In this diagram, `<operation>` represents either `Get`, `Update`, or `Delete`.

<picture>
  <source media="(prefers-color-scheme: dark)" srcset="./docs/figures/comment-cache-invalidation-low-level-dark.png">
  <img alt="Comment cache invalidation" src="./docs/figures/comment-cache-invalidation-low-level-light.png" width="700">
</picture>

## Running the System with Docker Swarm
> Run the following commands from the project root

### Build Service Images
```bash
docker build -t article-service:latest -f ArticleService/Dockerfile .
docker build -t comment-service:latest -f CommentService/Dockerfile .
docker build -t draft-service:latest -f DraftService/Dockerfile .
docker build -t newsletter-service:latest -f NewsletterService/Dockerfile .
docker build -t profanity-service:latest -f ProfanityService/Dockerfile .
docker build -t publisher-service:latest -f PublisherService/Dockerfile .
```

### Initialize Docker Swarm
```bash
docker swarm init
```

### Deploy Compose File as a Swarm Stack
```bash
docker stack deploy -c docker-compose.yml happy-headlines
```

## Inspecting and Verifying the Project
### View Nodes
```bash
docker node ls
```

### View Services
```bash
docker stack services happy-headlines
```

### View Containers
```bash
docker ps
```

### View Service Replicas
```bash
docker service ps happy-headlines_<service>
```

### Verify Tables
```bash
docker exec -it <container-id> psql -U happy-headlines -d <database> -c "\dt"
```

### Verify Data
```bash
docker exec -it <container-id> psql -U happy-headlines -d <database> -c "SELECT * FROM <table>;"
```

### Access Aspire Dashboard
```bash
docker service logs happy-headlines_aspire-dashboard 2>&1 | grep 'login?t='
```

## Stopping the Project
### Stop Stack
```bash
docker stack rm happy-headlines
```

### Delete Database Volumes
```bash
docker volume rm \
  happy-headlines_seq-data \
  happy-headlines_rabbitmq-data \
  happy-headlines_draft-database-data \
  happy-headlines_profanity-database-data \
  happy-headlines_comment-database-data \
  happy-headlines_article-database-africa-data \
  happy-headlines_article-database-antarctica-data \
  happy-headlines_article-database-asia-data \
  happy-headlines_article-database-europe-data \
  happy-headlines_article-database-north-america-data \
  happy-headlines_article-database-oceania-data \
  happy-headlines_article-database-south-america-data \
  happy-headlines_article-database-global-data 
```

### Leave Docker Swarm
```bash
docker swarm leave --force
```