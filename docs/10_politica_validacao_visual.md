# Politica de Validacao Visual Obrigatoria

## Regra principal

Depois de qualquer modificacao que afete gameplay, cena, camera, arte, UI, colisao, feedback visual ou integracao no Godot, a tarefa nao pode ser considerada concluida apenas com build C#.

E obrigatorio abrir o projeto no Godot e validar visualmente.

## Godot local

Executavel confirmado em 2026-07-07:

```text
C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe
```

Comando para abrir o projeto:

```powershell
Start-Process -FilePath 'C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe' -ArgumentList '--path','C:\Users\mober\Documents\Codex\2026-07-07\boa-cara-estamos-num-reposit-rio'
```

## Quando validar

Validacao visual e obrigatoria apos alteracoes em:

- `scenes/`
- `src/Prototype/`
- Scripts de player, inimigos, camera ou arena.
- Assets visuais em `assets/art/`.
- UI, HUD, fontes ou feedback visual.
- Shaders, particulas, tela, escala ou configuracao de janela.
- Qualquer alteracao que mude sensacao de combate.

## O que validar

Para a Sprint 1:

- `scenes/Main.tscn` abre sem erro critico.
- Player aparece e responde a `WASD`.
- `J` executa ataque leve.
- Hitbox conecta somente durante a janela ativa.
- Inimigos recebem hit flash, hit stun, knockback e morrem.
- Camera segue o player e o shake nao atrapalha a leitura.
- Player e inimigos respeitam os limites da faixa jogavel.
- A cena ainda comunica profundidade, mesmo com placeholders.

## Registro obrigatorio

Apos validar, atualizar:

- `docs/08_estado_atual.md`
- Documento da sprint atual em `docs/sprints/`
- `docs/checklists/validacao_visual.md`, se a regra mudar

Registrar sempre:

- Data da validacao.
- Cena validada.
- Resultado: aprovado, ajustar ou bloqueado.
- Observacoes visuais importantes.

## Politica para Codex e Cursor

Codex e Cursor podem implementar e compilar, mas nao devem declarar uma sprint como fechada sem registro de validacao visual no Godot.

Se o agente nao conseguir inspecionar visualmente a janela, deve marcar a validacao como pendente para o usuario confirmar.
