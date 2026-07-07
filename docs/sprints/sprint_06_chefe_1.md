# Sprint 6 - Chefe 1

## Objetivo

Criar o Capitao do Ferro como primeiro chefe memoravel com arena propria e recompensa narrativa.

## Entregas

- [x] `BossIronCaptain` com 3 padroes: corrente, esmagamento e investida.
- [x] `IronCaptainArena` com decoracao de ferro.
- [x] `BossDirector` com intro e vitoria.
- [x] `GameRoot` encadeando Aldeia -> arena do chefe.
- [x] Recompensa narrativa via `MemoryRegistry`.

## Validacao

- Build C# sem erros.
- Godot carrega `IronCaptainArena` e `BossIronCaptain`.
- Validacao tecnica automatica; interativa pendente para o usuario.

## Agentes

- Goku: 3 padroes distintos e legiveis.
- Gohan: intro e memoria da corrente com peso dramatico.
- Vegeta: hooks virtuais em `EnemyBase` para bosses.
- Trunks: fluxo de fases documentado em `GameRoot`.
