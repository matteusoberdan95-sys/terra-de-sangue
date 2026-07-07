# Sprint 3 - Sistema de Inimigos

## Objetivo

Transformar os bonecos em combate real: inimigos atacam, o player recebe dano, surge o inimigo bruto e as ondas escalam a pressao.

## Escopo

Entra:

- IA de aproximacao refinada com leitura de profundidade.
- Ataque inimigo com startup, janela ativa e recuperacao.
- Slot unico de ataque para evitar spam simultaneo.
- Janela de dano do player com hurtbox, vida, hit stun e respawn.
- Inimigo bruto mais lento, resistente e pesado.
- Spawn por ondas com label de progresso.

Nao entra:

- Arte final ou sprite sheet.
- Boss.
- Fase completa.
- Gore progressivo.
- Sons.

## Checklist tecnico

- [x] Cena `Main.tscn` continua abrindo.
- [x] Build .NET 10 sem erros.
- [x] Godot console sem erro de assembly/script.
- [x] Combate da Sprint 1 preservado no player.
- [x] `EnemyBase` centraliza IA compartilhada.
- [x] `EnemyDummy` vira mercenario leve.
- [x] `EnemyBrute` adicionado.
- [x] `WaveSpawner` com 3 ondas.
- [x] Player com hurtbox e `TakeHit`.

## Checklist de sensacao

- [x] Inimigos atacam quando chegam perto.
- [x] Apenas um inimigo ataca por vez na maioria dos casos.
- [x] Player sente o golpe com flash, stun e knockback.
- [x] Bruto parece mais perigoso que mercenario.
- [x] Ondas criam escalada de pressao.
- [x] Validacao visual humana no Godot.

## Registro de validacao visual

Status atual: validado localmente no Godot em 2026-07-07. Aprovado pelo usuario.

Comando usado para abrir o editor:

```powershell
Start-Process -FilePath 'C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe' -ArgumentList '--path','C:\Users\mober\OneDrive\Desktop\fauna-do-sangue'
```

Execucao automatica da cena:

```text
Godot --path fauna-do-sangue --verbose res://scenes/Main.tscn --quit-after 12
```

Resultado tecnico:

- Projeto abriu sem erro critico.
- .NET hostfxr inicializou e `TerraSangrada.dll` carregou.
- Scripts `PrototypeArena`, `PlayerController`, `WaveSpawner`, `EnemyBase` e `EnemyDummy` compilaram e instanciaram.
- Nenhum erro de assembly/script no console.
- Cena `scenes/Main.tscn` rodou por 12 segundos sem crash.

Resultado: aprovado pelo usuario em 2026-07-07.

Checklist confirmado:

- `WASD` move o player.
- `J` ataca e mata inimigos.
- Inimigos se aproximam e atacam quando perto.
- Player perde vida ao ser atingido.
- Player respawna apos morrer.
- Brutos aparecem a partir da onda 2.
- Label de onda atualiza corretamente.
- Camera e combate da Sprint 2 continuam legiveis.

## Agentes envolvidos

- Goku valida sensacao do combate e pressao das ondas.
- Vegeta valida arquitetura de `EnemyBase` e build.
- Piccolo valida leitura visual dos novos estados.
- Krillin valida regressao jogavel.
- Trunks atualiza estado e proximo passo.
