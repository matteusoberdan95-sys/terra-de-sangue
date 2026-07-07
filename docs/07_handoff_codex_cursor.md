# Handoff Codex e Cursor CI

Este documento existe para permitir que o projeto continue sem perda de contexto ao alternar entre Codex e Cursor.

## Entrada rapida

Ler nesta ordem:

1. `AGENTS.md`
2. `docs/08_estado_atual.md`
3. `docs/04_plano_de_sprints.md`
4. `docs/sprints/sprint_00_fundacao.md`
5. `docs/sprints/sprint_01_combate_prototipo.md`

## O que ja existe

- Projeto Godot inicial.
- Base C# compilando.
- Cena principal que instancia uma arena prototipo.
- Player placeholder com movimento e ataque leve.
- Inimigos placeholder com vida, hit flash, knockback e morte.
- Documentos de visao, arte, narrativa, gameplay, sprints e agentes.

## Onde mexer

- Gameplay prototipo: `src/Prototype/`
- Cenas: `scenes/`
- Direcao e planejamento: `docs/`
- Agentes: `.agents/`
- Regras do Cursor: `.cursor/rules/`
- Contexto Codex: `.codex/`

## Onde nao mexer sem motivo

- `.godot/`: gerado pelo Godot.
- `work/`: rascunhos locais.
- `outputs/`: entregaveis finais para o usuario.

## Validacao minima

```powershell
dotnet restore TerraSangrada.csproj
dotnet build TerraSangrada.csproj --no-restore
```

Depois, abrir no Godot .NET e rodar:

```text
res://scenes/Main.tscn
```

## Ao finalizar uma tarefa

Atualizar:

- Checklist da sprint atual.
- `docs/08_estado_atual.md`.
- Qualquer decisao nova em `docs/09_decisoes.md`.

Registrar no resumo final:

- Arquivos alterados.
- Validacao feita.
- Pendencias para a proxima ferramenta.
