# Codex Context - Terra Sangrada

## Missao

Construir Terra Sangrada por sprints, com Codex atuando como executor principal de organizacao, implementacao e validacao tecnica.

## Como continuar uma sessao

1. Ler `AGENTS.md`.
2. Ler `docs/08_estado_atual.md`.
3. Conferir `git status --short`.
4. Se houver mudancas, entender antes de editar.
5. Trabalhar apenas no objetivo da sprint atual.

## Sprint atual

Sprint 1 - Combate Prototipo.

Estado atual:

- Implementacao tecnica concluida.
- Build C# aprovado.
- Validacao visual no Godot ainda pendente.

Bloqueio atual para fechar:

- Abrir o projeto no Godot .NET.
- Rodar `scenes/Main.tscn`.
- Confirmar movimento, ataque, hitbox, knockback, camera shake e limites de arena.

## Decisoes ja tomadas

- Nome do jogo: Terra Sangrada.
- Estilo: hack and slash / beat 'em up 2D adulto.
- Base tecnica: Godot .NET + C#.
- Cultura do jogo: ficticia, inspirada no Brasil, evitando representacao rasa de povos reais.
- Processo: sprints com validacao visual obrigatoria.
- Agentes tem nomes inspirados em Dragon Ball Z para facilitar memoria operacional.
- Sprint 1 mantem placeholders; arte final fica para sprint posterior.

## Validacao tecnica ja feita

- `dotnet restore TerraSangrada.csproj`: sucesso.
- `dotnet build TerraSangrada.csproj --no-restore`: sucesso com 0 erros e 0 avisos quando executado com permissao fora do sandbox.
- Sprint 1 tambem compila com 0 erros e 0 avisos.
