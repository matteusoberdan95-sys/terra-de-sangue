# Gameplay e Combate

## Loop principal

Entrar em uma arena, ler inimigos, controlar espaco, quebrar defesa, finalizar, avancar, coletar memoria, enfrentar chefe.

## Controles iniciais

- `WASD`: movimento.
- `J`: ataque leve.

Controles futuros:

- Ataque pesado.
- Esquiva.
- Agarrao.
- Especial espiritual.
- Interacao.

## Regras de combate

- O eixo Y define profundidade, prioridade visual e posicionamento tatico.
- Ataques precisam ter antecipacao, impacto e recuperacao.
- Inimigos devem te cercar, mas nao atacar todos ao mesmo tempo.
- Dano deve ser muito legivel: flash, pausa curta, sangue, knockback e som.

## Kit inicial do protagonista

- Ataque leve rapido.
- Ataque pesado lento.
- Combo de 3 golpes.
- Agarrao em inimigo tonto.
- Execucao curta quando inimigo esta quase morto.
- Furia ancestral com duracao limitada.

## Estados minimos

Player:

- Idle
- Walk
- LightAttack
- HeavyAttack
- HitStun
- Grab
- Execute
- Dead

Enemy:

- Idle
- Approach
- Strafe
- Attack
- HitStun
- Knockdown
- Dead

## Criterio de qualidade

O combate so avanca para conteudo novo quando o prototipo basico ja parecer bom com placeholders. Se o jogo nao for divertido com formas simples, arte final nao vai salvar.
