# GroupStage Simulator

A mini simulator for a football group stage consisting of 4 teams. Simulates all matches, calculates standings with full tiebreaker support (points, goal difference, goals for, goals against, head-to-head), and determines which teams advance to the knockout stage.

## Run locally

```bash
cd StageSim.API
dotnet run
```

Swagger available at `http://localhost:7000/swagger`.

## Run with Docker

```bash
# Build from solution root
docker build -f StageSim.API/Dockerfile -t stagesim-api .

docker run -p 8080:8080 stagesim-api
```

## Deploy to k3s

```bash
kubectl apply -f k8s/deployment.yaml -n <namespace>
```

API available at `https://lontray.art/stagesim/groups/simulate`.

## Live demo

- **Frontend:** https://gray-mushroom-0dfd13410.2.azurestaticapps.net
- **API (Swagger):** https://lontray.art/stagesim/swagger

## API tests

HTTP test file available at `StageSim.API/HttpFiles/groupstage.http` (compatible with JetBrains Rider / VS Code REST Client).

## Run tests

```bash
dotnet test
```
