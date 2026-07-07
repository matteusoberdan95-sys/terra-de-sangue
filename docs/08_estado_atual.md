# Estado Atual

Ultima atualizacao: 2026-07-07.

## Projeto

- Nome: Terra Sangrada.
- Genero: hack and slash / beat 'em up 2D.
- Engine: Godot .NET 4.7.
- Linguagem: C#.
- Runtime local: .NET 10.0.
- Target framework do jogo: `net10.0`.
- Tom: adulto, dark, sangrento, melancolico.

## Sprint atual

Sprint 4 - Fase 1 Jogavel.

Status: implementada localmente, aguardando validacao visual no Godot.

Sprint 4 implementada localmente com:

- `AldeiaEmCinzas.tscn` como cena da Fase 1.
- `PhaseDirector` com intro, 3 encontros, mini-chefe, memoria e fim de fase.
- `EnemyMiniBoss` (Sargento do Ferro).
- `MemoryPickup` com primeira memoria: mascara quebrada.
- `WaveSpawner` removido em favor do fluxo de fase.

## Implementado

- Projeto Godot e C#.
- Cena principal `scenes/Main.tscn` carregando `AldeiaEmCinzas.tscn`.
- Arena visual da Aldeia em Cinzas.
- Player com movimento, ataque, dano e respawn.
- Inimigos mercenario, bruto e mini-chefe.
- Sistema de fase com encontros escalonados.
- Primeira memoria coletavel.
- Documentos de producao e agentes.

## Validacao feita

- `dotnet build TerraSangrada.csproj --no-restore`: sucesso, 0 erros e 0 avisos.
- Sprint 4: Godot aberto via `Start-Process` documentado em 2026-07-07.
- Sprint 4: `res://scenes/Main.tscn` executada por 15s com `--verbose`, sem erro de assembly/script.
- Sprint 4: validacao visual interativa pendente de confirmacao humana.

Godot local:

```text
C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe
```

Repositorio local principal:

```text
C:\Users\mober\OneDrive\Desktop\fauna-do-sangue
```

## Proximo passo

Validar Sprint 4 visualmente no Godot local. Depois commitar, dar push e iniciar Sprint 5: Polimento de Impacto.

## Fechamento da Sprint 3

- Commit: `7259c7a` / `6f2ad4c` — enviado para `origin/main`.

## Riscos atuais

- Godot nao esta no PATH; abre por caminho absoluto local.
- Memoria ainda nao desbloqueia habilidade jogavel.
- Fase termina em banner, sem transicao para a proxima area.
