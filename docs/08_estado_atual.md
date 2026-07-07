# Estado Atual

Ultima atualizacao: 2026-07-07.

## Projeto

- Nome: Terra Sangrada.
- Genero: hack and slash / beat 'em up 2D.
- Engine: Godot .NET 4.x.
- Linguagem: C#.
- Tom: adulto, dark, sangrento, melancolico.

## Sprint atual

Sprint 0 - Fundacao.

Status: quase concluida.

Falta para fechar:

- Abrir o projeto no Godot .NET.
- Rodar `scenes/Main.tscn`.
- Validar visualmente que a cena inicial aparece corretamente.
- Marcar o checklist final de Sprint 0.

## Implementado

- Projeto Godot criado.
- Projeto C# criado.
- Cena principal `scenes/Main.tscn`.
- Arena prototipo `scenes/levels/PrototypeArena.tscn`.
- Player placeholder com movimento `WASD`.
- Ataque leve em `J`.
- Inimigos placeholder com vida, hit flash, knockback e morte.
- Camera seguindo o player.
- Documentos de producao e agentes.

## Validacao feita

- `dotnet restore TerraSangrada.csproj`: sucesso.
- `dotnet build TerraSangrada.csproj --no-restore`: sucesso, 0 erros e 0 avisos, executado fora do sandbox por permissao.

## Proximo passo

Validar visualmente a Sprint 0 no Godot. Se aprovado, iniciar Sprint 1: combate prototipo.

## Riscos atuais

- Godot nao foi encontrado no PATH do ambiente atual.
- Validacao visual ainda depende de abrir o editor Godot .NET localmente.
- Hitbox do ataque ainda esta simplificada por distancia, nao por Area2D dedicada.
