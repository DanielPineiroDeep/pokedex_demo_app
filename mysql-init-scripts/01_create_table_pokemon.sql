CREATE DATABASE IF NOT EXISTS pruebapokeapi_db;
USE pruebapokeapi_db;

CREATE TABLE IF NOT EXISTS Pokemon (
    n_dex INT PRIMARY KEY,
    nombre VARCHAR(30) NOT NULL,
    t_primario VARCHAR(20) NOT NULL,
    t_secundario VARCHAR(20),
    altura VARCHAR(10) NOT NULL,
    peso VARCHAR(10) NOT NULL
);