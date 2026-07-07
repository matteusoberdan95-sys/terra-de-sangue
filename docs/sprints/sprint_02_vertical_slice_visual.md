# Sprint 2 - Vertical Slice Visual

## Objetivo

Substituir a leitura de placeholder por uma primeira identidade visual jogavel para Terra Sangrada, mantendo o combate validado na Sprint 1.

## Escopo

Entra:

- Protagonista com silhueta mais forte.
- Inimigo mercenario com leitura de arma/metal.
- Fundo da Aldeia em Cinzas com camadas.
- Brasas e cinzas animadas.
- Paleta da biblia de arte aplicada na cena.

Nao entra:

- Arte final bitmap.
- Sprite sheet completo.
- Boss.
- Fase completa.
- IA nova de inimigos.

## Checklist tecnico

- [x] Cena `Main.tscn` continua abrindo.
- [x] Build .NET 10 sem erros.
- [x] Godot console sem erro de assembly/script.
- [x] Combate da Sprint 1 preservado.
- [x] Arena continua visivel no editor do Godot.

## Checklist visual

- [x] Fundo tem leitura de aldeia queimada.
- [x] Faixa jogavel continua clara.
- [x] Protagonista tem silhueta mais reconhecivel.
- [x] Mercenario tem arma e metal visiveis.
- [x] Brasas/cinzas tem movimento em runtime.
- [x] Validacao visual humana no Godot.

## Registro de validacao visual

Status atual: validado localmente no Godot.

Cena para validar:

```text
scenes/Main.tscn
```

Checklist de teste:

- `WASD` move o player.
- `J` ataca.
- Inimigos recebem dano e morrem.
- Camera continua seguindo bem.
- A nova arte nao esconde a leitura do combate.
- Brasas/cinzas aparecem sem atrapalhar.

Resultado: aprovado pelo usuario em 2026-07-07.

Observacoes:

- Vertical slice visual aprovado localmente antes de commit/push.
- Direcao gore adulta registrada como pilar do projeto em `docs/11_direcao_de_violencia_gore.md`.

## Agentes envolvidos

- Piccolo valida direcao de arte e leitura visual.
- Goku valida se a sensacao do combate foi preservada.
- Vegeta valida build e estrutura tecnica.
- Krillin valida regressao jogavel.
- Trunks atualiza estado e proximo passo.
