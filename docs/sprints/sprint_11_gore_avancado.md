# Sprint 11 - Gore Avancado

## Objetivo

Transformar a brutalidade em sistema jogavel com desmembramento por arma, execucoes contextuais e sangue em vegetacao, sem perder legibilidade.

## Entregas

- [x] `PlayerAttackKind` e `ExecutionStyle` para contextualizar golpes.
- [x] `SeveredLimb` com membros voadores (braco, cabeca, torso).
- [x] Desmembramento por tipo de golpe: leve, combo, pesado.
- [x] Execucoes contextuais com `E`: decapitacao, estocada visceral, esmagamento de cranio.
- [x] Feridas progressivas avancadas: rasgo profundo e membro ferido.
- [x] `VegetationBloodStain` e props de vegetacao na Mata Fechada.
- [x] `GoreReadability` limita splatters, decals e membros ativos.

## Controles

- `J` / `J`+`J` / `K`: desmembramento diferente em mortes fatais.
- `E`: execucao contextual (varia por inimigo).

## Validacao

- Build C# sem erros.
- Godot carrega cena e scripts de gore avancado.

## Agentes

- Goku: desmembramento reforca peso sem atrapalhar leitura.
- Piccolo: sangue contrasta com verde da mata.
- Vegeta: caps de efeitos em `GoreReadability`.
- Gohan: execucoes com peso dramatico distinto.
- Krillin: regressao do combate basico.

## Registro de validacao visual

Status atual: validado localmente no Godot em 2026-07-07. Aprovado pelo usuario.

Resultado: aprovado pelo usuario em 2026-07-07.

Cena para validar:

```text
scenes/Main.tscn
```

Checklist de teste interativo:

- Morte com `J` arranca braco.
- Morte com combo `J`+`J` decapita.
- Morte com `K` gera torso/partes extras.
- `E` em inimigo com 1 HP dispara estilo contextual.
- Mata Fechada mostra manchas de sangue na vegetacao proxima.
- Tela nao fica poluida demais em lutas longas.
