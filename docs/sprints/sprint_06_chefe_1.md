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

## Registro de validacao visual

Status atual: validado localmente no Godot em 2026-07-07. Aprovado pelo usuario.

Resultado: aprovado pelo usuario em 2026-07-07.

Cena para validar:

```text
scenes/Main.tscn
```

Checklist de teste interativo:

- Transicao Aldeia -> arena do chefe.
- Intro do Capitao do Ferro.
- Padroes corrente, esmagamento e investida legiveis.
- Vitoria desbloqueia memoria e proxima fase.

## Agentes

- Goku: 3 padroes distintos e legiveis.
- Gohan: intro e memoria da corrente com peso dramatico.
- Vegeta: hooks virtuais em `EnemyBase` para bosses.
- Trunks: fluxo de fases documentado em `GameRoot`.
