# Handoff Codex e Cursor CI

Este documento existe para permitir que o projeto continue sem perda de contexto ao alternar entre Codex e Cursor.

## Entrada rapida

Ler nesta ordem:

1. `AGENTS.md`
2. `docs/08_estado_atual.md`
3. `docs/04_plano_de_sprints.md`
4. `docs/sprints/sprint_01_combate_prototipo.md`
5. `docs/10_politica_validacao_visual.md`
6. `docs/checklists/validacao_visual.md`

## O que ja existe

- Projeto Godot inicial.
- Base C# compilando.
- Cena principal que instancia uma arena prototipo.
- Player placeholder com movimento e ataque leve.
- Ataque leve com hitbox separada da colisao do player.
- Inimigos placeholder com hurtbox, vida, hit flash, knockback, hit stun e morte.
- Camera com primeiro passe de shake.
- Hit pause inicial em golpes conectados.
- Area jogavel limitada no eixo Y.
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

Godot local confirmado:

```text
C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe
```

Validar especificamente:

- `WASD` move o player.
- `J` executa ataque leve.
- Ataque acerta apenas pela hitbox ativa.
- Inimigos recebem dano, recuam e morrem.
- Camera treme sem perder enquadramento.
- Player e inimigos nao saem da faixa jogavel.

## Ao finalizar uma tarefa

Atualizar:

- Checklist da sprint atual.
- `docs/08_estado_atual.md`.
- Qualquer decisao nova em `docs/09_decisoes.md`.
- Registro de validacao visual quando a tarefa alterar gameplay, cena, camera, arte ou UI.

Antes de iniciar a proxima sprint:

- Criar commit local da sprint validada.
- Dar push para o GitHub.
- Confirmar que o remoto esta atualizado.

Registrar no resumo final:

- Arquivos alterados.
- Validacao feita.
- Commit e push realizados.
- Pendencias para a proxima ferramenta.
