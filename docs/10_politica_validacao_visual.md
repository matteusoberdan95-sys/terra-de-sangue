# Politica de Validacao Visual Obrigatoria

## Regra principal

Depois de qualquer modificacao que afete gameplay, cena, camera, arte, UI, colisao, feedback visual ou integracao no Godot, a tarefa nao pode ser considerada concluida apenas com build C#.

E obrigatorio abrir o projeto no Godot e validar visualmente.

## Ordem obrigatoria de trabalho

1. Fazer a alteracao localmente em:

```text
C:\Users\mober\OneDrive\Desktop\fauna-do-sangue
```

2. Compilar localmente.
3. Abrir no Godot local.
4. Validar visualmente a cena afetada.
5. Registrar o resultado nos documentos da sprint.
6. Somente depois criar commit e subir para o GitHub.
7. Somente depois iniciar a proxima sprint.

Nunca subir para o repositorio remoto antes da validacao visual quando a alteracao afetar gameplay, cena, camera, arte ou UI.

Nunca iniciar a proxima sprint sem commit e push da sprint validada no GitHub.

## Politica de commit e push obrigatorio

Assim como a validacao visual, commit e push fazem parte do fechamento da sprint — nao sao etapas opcionais.

Regra principal:

- A sprint so pode ser considerada encerrada depois de validada, commitada e enviada ao remoto.
- Codex, Cursor e qualquer agente devem commitar e dar push antes de abrir a proxima sprint.
- Se a sprint foi validada mas ainda nao foi para o GitHub, a proxima sprint fica bloqueada.

Comandos minimos:

```powershell
git add .
git commit -m "mensagem da sprint"
git push
```

Registrar no fechamento:

- Hash ou mensagem do commit.
- Confirmacao de que o push chegou ao remoto.

## Godot local

Executavel confirmado em 2026-07-07:

```text
C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe
```

Versao confirmada:

```text
4.7.stable.mono.official.5b4e0cb0f
```

Runtime .NET local confirmado:

```text
Microsoft.NETCore.App 10.0.1
```

O projeto usa `net10.0` por decisao tecnica do projeto. A validacao local precisa confirmar que o Godot 4.7 Mono carrega a assembly `TerraSangrada.dll` sem erros.

Configuracao obrigatoria no `project.godot`:

```text
[dotnet]
project/assembly_name="TerraSangrada"
```

Comando para abrir o projeto:

```powershell
Start-Process -FilePath 'C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe' -ArgumentList '--path','C:\Users\mober\Documents\Codex\2026-07-07\boa-cara-estamos-num-reposit-rio'
```

Comando para abrir a copia local principal no Desktop:

```powershell
Start-Process -FilePath 'C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe' -ArgumentList '--path','C:\Users\mober\OneDrive\Desktop\fauna-do-sangue'
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

## Validacoes registradas

### 2026-07-07 - Sprint 1

- Projeto local: `C:\Users\mober\OneDrive\Desktop\fauna-do-sangue`
- Cena: `scenes/Main.tscn`
- Resultado: aprovado pelo usuario.
- Build: `dotnet build TerraSangrada.csproj --no-restore` com 0 erros e 0 avisos.
- Godot console: sem erro de assembly/script apos corrigir `project/assembly_name`.

### 2026-07-07 - Sprint 2

- Projeto local: `C:\Users\mober\OneDrive\Desktop\fauna-do-sangue`
- Cena: `scenes/Main.tscn`
- Resultado: aprovado pelo usuario.
- Build: `dotnet build TerraSangrada.csproj --no-restore` com 0 erros e 0 avisos.
- Conteudo validado: Aldeia em Cinzas, silhuetas refinadas, mercenarios com arma/metal, brasas/cinzas animadas e direcao gore registrada.

### 2026-07-07 - Sprint 3

- Projeto local: `C:\Users\mober\OneDrive\Desktop\fauna-do-sangue`
- Cena: `scenes/Main.tscn`
- Resultado: aprovado pelo usuario.
- Build: `dotnet build TerraSangrada.csproj --no-restore` com 0 erros e 0 avisos.
- Conteudo validado: ataque inimigo, dano no player, inimigo bruto, ondas e regressao do combate.

### 2026-07-07 - Sprint 3 (tecnica)

- Projeto local: `C:\Users\mober\OneDrive\Desktop\fauna-do-sangue`
- Cena: `scenes/Main.tscn`
- Editor aberto via `Start-Process` conforme documentado.
- Execucao automatica: `--verbose res://scenes/Main.tscn --quit-after 12`
- Resultado tecnico: aprovado — .NET, scripts C# e cena carregaram sem erro.
- Pendencia: confirmacao humana do gameplay interativo (WASD, J, dano, ondas).

### 2026-07-07 - Sprints 6 a 10

- Projeto local: `C:\Users\mober\OneDrive\Desktop\fauna-do-sangue`
- Cena: `scenes/Main.tscn`
- Resultado: aprovado pelo usuario.
- Build: `dotnet build TerraSangrada.csproj --no-restore` com 0 erros e 0 avisos.
- Conteudo validado:
  - Sprint 6: Capitao do Ferro, 3 padroes, arena, transicao Aldeia -> chefe.
  - Sprint 7: sangue persistente, execucao com `E`, decals no chao.
  - Sprint 8: ataque pesado `K`, combo `J`+`J`, feedback diferenciado.
  - Sprint 9: Mata Fechada, 2 encontros, memoria semente negra.
  - Sprint 10: `CombatHud` com vida, barra do chefe e contador de memorias.

### 2026-07-07 - Sprint 11

- Projeto local: `C:\Users\mober\OneDrive\Desktop\fauna-do-sangue`
- Cena: `scenes/Main.tscn`
- Resultado: aprovado pelo usuario.
- Build: `dotnet build TerraSangrada.csproj --no-restore` com 0 erros e 0 avisos.
- Conteudo validado: desmembramento por arma, execucoes contextuais, feridas avancadas, sangue em vegetacao, caps de legibilidade.

### 2026-07-07 - Sprint 12

- Projeto local: `C:\Users\mober\OneDrive\Desktop\fauna-do-sangue`
- Cena: `scenes/Main.tscn`
- Resultado: aprovado pelo usuario.
- Build: `dotnet build TerraSangrada.csproj --no-restore` com 0 erros e 0 avisos.
- Conteudo validado: silhuetas compostas, VisualRig com animacao, props da aldeia, memoria e banners da fase 1.

### 2026-07-07 - Sprint 13

- Projeto local: `C:\Users\mober\OneDrive\Desktop\fauna-do-sangue`
- Cena: `scenes/Main.tscn`
- Resultado: aprovado pelo usuario.
- Conteudo validado: telegraph, hitstop, knockback, encontros rebalanceados.

### 2026-07-07 - Sprint 14

- Projeto local: `C:\Users\mober\OneDrive\Desktop\fauna-do-sangue`
- Cena: `scenes/Main.tscn`
- Resultado: aprovado pelo usuario.
- Conteudo validado: CombatAudio, telegraph sonoro, impactos diferenciados, memoria e mini-chefe.
- Nota: placeholders procedurais mantidos ate pack SFX custom em `assets/audio/`.

### 2026-07-07 - Sprint 15

- Cena: `scenes/Main.tscn`
- Resultado: aprovado pelo usuario.
- Conteudo validado: sprites pixel Arandu e mercenario, AudioLibrary.

### 2026-07-07 - Sprint 16

- Cena: `scenes/Main.tscn`
- Resultado: aprovado pelo usuario.
- Conteudo validado: esquiva com Space, barra de stamina, contra-ataque pos-dodge, cancel de recovery, SFX de esquiva.

## Politica para Codex e Cursor

Codex e Cursor podem implementar e compilar, mas nao devem declarar uma sprint como fechada sem registro de validacao visual no Godot.

Codex e Cursor tambem nao devem iniciar a proxima sprint sem commit e push da sprint validada no GitHub.

Se o agente nao conseguir inspecionar visualmente a janela, deve marcar a validacao como pendente para o usuario confirmar.
