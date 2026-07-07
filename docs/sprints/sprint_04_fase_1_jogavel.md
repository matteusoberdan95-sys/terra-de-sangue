# Sprint 4 - Fase 1 Jogavel

## Objetivo

Montar a primeira fase curta da Aldeia em Cinzas do inicio ao fim, com encontros escalonados, mini-chefe e primeira memoria coletavel.

## Escopo

Entra:

- Fase 1 jogavel da Aldeia em Cinzas.
- Intro e fim de fase com banners.
- 3 encontros de combate.
- 1 mini-chefe: Sargento do Ferro.
- Primeira memoria coletavel: mascara quebrada.
- Cena dedicada `AldeiaEmCinzas.tscn`.

Nao entra:

- Arte final ou sprite sheet.
- Chefe principal (Capitao do Ferro).
- Nova habilidade jogavel da memoria.
- Transicao para Fase 2.
- Sons.

## Checklist tecnico

- [x] Cena `Main.tscn` continua abrindo.
- [x] Build .NET 10 sem erros.
- [ ] Godot console sem erro de assembly/script.
- [x] `AldeiaEmCinzas.tscn` instancia a arena.
- [x] `PhaseDirector` substitui `WaveSpawner`.
- [x] `EnemyMiniBoss` adicionado.
- [x] `MemoryPickup` adicionado.
- [x] Combate das sprints anteriores preservado.

## Checklist de sensacao

- [ ] Intro comunica a fase.
- [ ] 3 encontros escalam pressao.
- [ ] Mini-chefe parece mais perigoso que brutos.
- [ ] Memoria aparece apos o mini-chefe.
- [ ] Coletar memoria dispara outro narrativo.
- [ ] Fim de fase fica claro.
- [ ] Validacao visual humana no Godot.

## Registro de validacao visual

Status atual: validado localmente no Godot em 2026-07-07. Aprovado pelo usuario.

Comando usado para abrir o editor:

```powershell
Start-Process -FilePath 'C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe' -ArgumentList '--path','C:\Users\mober\OneDrive\Desktop\fauna-do-sangue'
```

Execucao automatica da cena:

```text
Godot --path fauna-do-sangue --verbose res://scenes/Main.tscn --quit-after 15
```

Resultado tecnico:

- `AldeiaEmCinzas.tscn` carregou via `Main.tscn`.
- `PhaseDirector`, `PrototypeArena` e `PlayerController` instanciaram sem erro.
- Nenhum erro de assembly/script no console.

Checklist de teste interativo (confirmar no editor com F5):

- Intro "Aldeia em Cinzas" aparece no inicio.
- 3 encontros acontecem em sequencia.
- Mini-chefe aparece apos o terceiro encontro.
- Memoria pulsa apos derrotar o mini-chefe.
- Coletar memoria mostra texto da mascara quebrada.
- Banner de fase concluida aparece no fim.
- `WASD` e `J` continuam funcionando.

## Agentes envolvidos

- Goku valida fluxo da fase e escalada dos encontros.
- Gohan valida tom da memoria e banners.
- Vegeta valida arquitetura de `PhaseDirector` e build.
- Piccolo valida leitura visual dos novos elementos.
- Krillin valida regressao jogavel.
- Trunks atualiza estado e proximo passo.
