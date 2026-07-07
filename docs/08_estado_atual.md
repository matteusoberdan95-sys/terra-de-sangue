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

Sprint 3 - Sistema de Inimigos.

Status: validada localmente no Godot.

Sprint 3 fechada com:

- `EnemyBase` com IA compartilhada de aproximacao e ataque.
- Slot unico de ataque para evitar spam simultaneo.
- `EnemyDummy` como mercenario leve.
- `EnemyBrute` mais lento, resistente e pesado.
- Player com hurtbox, vida, hit stun, invulnerabilidade pos-respawn e respawn.
- `WaveSpawner` com 3 ondas progressivas e label de progresso.
- Validacao visual aprovada pelo usuario em 2026-07-07.

## Implementado

- Projeto Godot criado.
- Projeto C# criado.
- Cena principal `scenes/Main.tscn`.
- Arena prototipo `scenes/levels/PrototypeArena.tscn`.
- Player com movimento `WASD`, ataque leve em `J`, hurtbox e vida.
- Estados de player: idle, walk, light attack, hit stun, dead.
- Hitbox de ataque separada da colisao do player.
- Inimigos com hurtbox, vida, hit flash, hit stun, knockback, morte e ataque.
- Inimigos se aproximam e atacam com leitura de profundidade.
- Camera seguindo o player.
- Hit pause e screen shake.
- Limites de movimento no eixo Y.
- Primeiro passe visual da Aldeia em Cinzas.
- Silhuetas refinadas de Arandu e mercenarios.
- Brasas e cinzas animadas na arena.
- Sistema de ondas com mercenarios e brutos.
- Documentos de producao e agentes.

## Validacao feita

- `dotnet restore TerraSangrada.csproj`: sucesso.
- `dotnet build TerraSangrada.csproj --no-restore`: sucesso, 0 erros e 0 avisos.
- Sprint 3: Godot aberto via `Start-Process` documentado em 2026-07-07.
- Sprint 3: `res://scenes/Main.tscn` executada sem erro de assembly/script.
- Sprint 3: validacao visual aprovada pelo usuario em 2026-07-07.

Godot local:

```text
C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe
```

Repositorio local principal para desenvolvimento e validacao:

```text
C:\Users\mober\OneDrive\Desktop\fauna-do-sangue
```

## Proximo passo

Commitar Sprint 3 validada, dar push para o GitHub e somente depois iniciar Sprint 4: Fase 1 Jogavel.

## Riscos atuais

- Godot nao esta no PATH; abre por caminho absoluto local.
- Inimigos spawnados em runtime ainda usam silhueta procedural simples.
- Player ainda nao tem HUD de vida; feedback e por flash/stun/respawn.
