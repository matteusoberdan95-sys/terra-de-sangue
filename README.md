# Terra Sangrada

**Terra Sangrada** e um hack and slash / beat 'em up 2D adulto, sombrio e violento, inspirado por arcades dos anos 90 como *Final Fight* e *Streets of Rage*, com uma direcao artistica brasileira, melancolica e mitica.

O projeto sera construido em sprints curtas. Cada sprint deve terminar com uma validacao visual dentro do Godot antes da proxima comecar.

## Pilares

- Combate pesado, legivel e brutal.
- Arte 2D refinada, com leitura de arcade classico e acabamento moderno.
- Brasil ficcional, dark, sangrento e melancolico.
- Cultura indigena ficticia tratada com respeito, evitando caricaturas e misturas aleatorias.
- Progresso por cenas pequenas, testaveis e polidas.

## Stack

- Godot .NET 4.x
- C#
- Camera 2D com profundidade beat 'em up
- Assets gerados por IA e refinados por sprint

## Como abrir

1. Abra o Godot .NET.
2. Importe a pasta deste projeto.
3. Abra `project.godot`.
4. Rode a cena principal `scenes/Main.tscn`.

Godot local confirmado:

`C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe`

## Organizacao

- `AGENTS.md`: guia obrigatorio para Codex, Cursor e agentes.
- `.agents/`: perfis dos agentes de validacao.
- `.codex/`: contexto de continuidade para o Codex.
- `.cursor/`: regras de continuidade para Cursor CI.
- `docs/`: visao, arte, narrativa, gameplay e plano de sprints.
- `scenes/`: cenas Godot.
- `src/`: scripts C#.
- `assets/`: arte, audio, fontes e materiais.
- `outputs/`: entregaveis finais gerados durante o trabalho.

## Agentes

O projeto usa agentes de validacao com apelidos de Dragon Ball Z:

- Goku: gameplay e combate.
- Vegeta: arquitetura e seguranca tecnica.
- Piccolo: direcao de arte.
- Gohan: narrativa e cuidado cultural.
- Bulma: ferramentas, Godot, C# e Cursor CI.
- Trunks: continuidade e backlog.
- Krillin: QA e playtest.

Detalhes em `docs/06_agentes.md`.

## Handoff Codex/Cursor

Para continuar o projeto em outra ferramenta, leia nesta ordem:

1. `AGENTS.md`
2. `docs/08_estado_atual.md`
3. `docs/07_handoff_codex_cursor.md`
4. `docs/04_plano_de_sprints.md`
5. `docs/10_politica_validacao_visual.md`

## Validacao Visual

Depois de qualquer alteracao em gameplay, cena, camera, arte ou UI, e obrigatorio abrir o projeto no Godot e validar visualmente antes de considerar a sprint concluida.

Detalhes em `docs/10_politica_validacao_visual.md`.

## Proxima sprint

Sprint 1: prototipo de combate em arena.

O objetivo nao e fazer uma fase bonita ainda. O objetivo e provar que andar, bater, receber impacto, derrubar inimigo e controlar camera ja parecem bons.
