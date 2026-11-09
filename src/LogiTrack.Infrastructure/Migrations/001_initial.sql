CREATE TABLE IF NOT EXISTS deliveries (
    id TEXT PRIMARY KEY,
    order_id TEXT NOT NULL,
    route_id TEXT NOT NULL,
    courier_id TEXT NULL,
    status INTEGER NOT NULL,
    version INTEGER NOT NULL,
    updated_at TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS outbox_events (
    id TEXT PRIMARY KEY,
    delivery_id TEXT NOT NULL,
    event_type TEXT NOT NULL,
    payload TEXT NOT NULL,
    occurred_at TEXT NOT NULL,
    attempts INTEGER NOT NULL DEFAULT 0,
    published_at TEXT NULL,
    last_error TEXT NULL
);

CREATE INDEX IF NOT EXISTS ix_outbox_pending
    ON outbox_events(delivery_id, published_at, attempts);

CREATE TABLE IF NOT EXISTS idempotency_records (
    request_id TEXT PRIMARY KEY,
    result_json TEXT NOT NULL,
    created_at TEXT NOT NULL
);

