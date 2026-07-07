# Estado Atual

Ultima atualizacao: 2026-07-07.

## Projeto

- Nome: Terra Sangrada.
- Genero: hack and slash / beat 'em up 2D.
- Engine: Godot .NET 4.x.
- Linguagem: C#.
- Tom: adulto, dark, sangrento, melancolico.

## Sprint atual

Sprint 1 - Combate Prototipo.

Status: implementada tecnicamente, aguardando validacao visual.

Falta para fechar:

- Abrir o projeto no Godot .NET.
- Rodar `scenes/Main.tscn`.
- Validar visualmente movimento, ataque, knockback, hit pause, camera shake e limites da arena.
- Marcar o checklist final de Sprint 1.

## Implementado

- Projeto Godot criado.
- Projeto C# criado.
- Cena principal `scenes/Main.tscn`.
- Arena prototipo `scenes/levels/PrototypeArena.tscn`.
- Player placeholder com movimento `WASD`.
- Ataque leve em `J`.
- Estados basicos de player: idle, walk e light attack.
- Hitbox de ataque separada da colisao do player.
- Inimigos placeholder com hurtbox, vida, hit flash, hit stun, knockback e morte.
- Inimigos se aproximam lentamente do player.
- Camera seguindo o player.
- Primeiro passe de hit pause e screen shake.
- Limites de movimento no eixo Y.
- Documentos de producao e agentes.

## Validacao feita

- `dotnet restore TerraSangrada.csproj`: sucesso.
- `dotnet build TerraSangrada.csproj --no-restore`: sucesso, 0 erros e 0 avisos, executado fora do sandbox por permissao.
- Godot nao foi encontrado no PATH nem em caminhos comuns pelo terminal.

## Proximo passo

Validar visualmente a Sprint 1 no Godot. Se aprovado, iniciar Sprint 2: vertical slice visual.

## Riscos atuais

- Godot nao foi encontrado no PATH do ambiente atual.
- Validacao visual ainda depende de abrir o editor Godot .NET localmente.
- Inimigos ainda nao atacam; isso fica para a sprint de sistema de inimigos.
