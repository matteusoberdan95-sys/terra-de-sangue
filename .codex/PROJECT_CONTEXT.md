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

Sprint 3 - Sistema de Inimigos validada localmente no Godot.

Estado atual:

- Sprint 1, 2 e 3 validadas visualmente pelo usuario em 2026-07-07.
- Proxima etapa: iniciar Sprint 4: Fase 1 Jogavel.
- Sprint 3 commitada e enviada ao remoto em `7259c7a`.

Godot local confirmado:

```text
C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe
```

Regra obrigatoria: seguir `docs/10_politica_validacao_visual.md` apos qualquer modificacao que afete gameplay, cena, camera, arte ou UI.

Regra obrigatoria: commitar e dar push da sprint validada antes de iniciar a proxima sprint.

## Decisoes ja tomadas

- Nome do jogo: Terra Sangrada.
- Estilo: hack and slash / beat 'em up 2D adulto.
- Base tecnica: Godot .NET 4.7 + C# / .NET 10.
- Cultura do jogo: ficticia, inspirada no Brasil, evitando representacao rasa de povos reais.
- Processo: sprints com validacao visual obrigatoria.
- Agentes tem nomes inspirados em Dragon Ball Z para facilitar memoria operacional.
- Sprint 1 mantem placeholders; arte final fica para sprint posterior.
- Identidade gore 18+ registrada em `docs/11_direcao_de_violencia_gore.md`.

## Validacao tecnica ja feita

- `dotnet restore TerraSangrada.csproj`: sucesso.
- `dotnet build TerraSangrada.csproj --no-restore`: sucesso com 0 erros e 0 avisos quando executado com permissao fora do sandbox.
- Sprint 1 tambem compila com 0 erros e 0 avisos.
- Sprint 1 validada visualmente no Godot local.
- Sprint 2 compila com 0 erros e 0 avisos.
- Sprint 2 validada visualmente no Godot local.
